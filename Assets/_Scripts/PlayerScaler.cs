using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScaler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentAxisText;

    [SerializeField] private Color xColor;
    [SerializeField] private Color yColor;
    [SerializeField] private Color zColor;

    private PlayerInputs _input;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public Axis currentAxis;

    private void Awake()
    {
        _input = GetComponent<PlayerInputs>();
    }

    public void OnSetAxisX(InputValue value)
    {
        currentAxis = Axis.X;
        currentAxisText.text = currentAxis.ToString();
        currentAxisText.color = xColor;
    }

    public void OnSetAxisY(InputValue value)
    {
        currentAxis = Axis.Y;
        currentAxisText.text = currentAxis.ToString();
        currentAxisText.color = yColor;
    }

    public void OnSetAxisZ(InputValue value)
    {
        currentAxis = Axis.Z;
        currentAxisText.text = currentAxis.ToString();
        currentAxisText.color = zColor;
    }

    private void Update()
    {
        if (_input.expand)
        {
            switch (currentAxis)
            {
                case Axis.X:
                    if (transform.localScale.x <= 2f)
                        transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y, transform.localScale.z);
                    break;

                case Axis.Y:
                    if (transform.localScale.y <= 2f)
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.05f, transform.localScale.z);
                    break;

                case Axis.Z:
                    if (transform.localScale.z <= 2f)
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + 0.05f);
                    break;
            }
        }

        if (_input.shrink)
        {
            switch (currentAxis)
            {
                case Axis.X:
                    if (transform.localScale.x >= 0.25f)
                        transform.localScale = new Vector3(transform.localScale.x - 0.05f, transform.localScale.y, transform.localScale.z);
                    break;

                case Axis.Y:
                    if (transform.localScale.y >= 0.25f)
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.05f, transform.localScale.z);
                    break;

                case Axis.Z:
                    if (transform.localScale.z >= 0.25f)
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - 0.05f);
                    break;
            }
        }
    }

    private void AdjustScaleOnAxis()
    {

    }
}
