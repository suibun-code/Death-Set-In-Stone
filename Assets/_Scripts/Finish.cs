using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private string nextScene;
    [SerializeField] private float finishWaitTime;
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject niceText;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.Play();
            timer.countdown = false;
            niceText.SetActive(true);
            StartCoroutine(OnFinishWaitTime());
        }
    }

    IEnumerator OnFinishWaitTime()
    {
        yield return new WaitForSeconds(finishWaitTime);

        SceneManager.LoadScene(nextScene);
    }
}
