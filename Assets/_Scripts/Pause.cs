using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool gamePaused = false;

    public void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            if (!gamePaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        pauseMenu.SetActive(false);
        Timer.instance.countdown = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        pauseMenu.SetActive(true);
        Timer.instance.countdown = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
