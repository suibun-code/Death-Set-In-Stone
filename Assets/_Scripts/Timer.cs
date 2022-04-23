using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    [SerializeField] private float startTime = 10f;
    private float timer;

    public bool countdown = true;

    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        timer = startTime;
    }

    void Update()
    {
        if (!countdown)
            return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            RespawnPlayer.instance.Respawn();
            timer = startTime;
        }

        SetTimerText();
    }

    private void SetTimerText()
    {
        timerText.text = timer.ToString("F2");
    }
}
