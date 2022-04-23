using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Change(string sceneName)
    {
        Pause.gamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
