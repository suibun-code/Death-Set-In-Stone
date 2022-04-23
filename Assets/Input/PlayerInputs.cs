using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    [Header("Character")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool expand;
    public bool shrink;
    public bool restart;

    [Header("Mouse")]
    public bool cursorLocked = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnExpand(InputValue value)
    {
        ExpandInput(value.isPressed);
    }

    public void OnShrink(InputValue value)
    {
        ShrinkInput(value.isPressed);
    }

    public void OnRestart(InputValue value)
    {
        RestartInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }

    public void ExpandInput(bool newExpandState)
    {
        expand = newExpandState;
    }

    public void ShrinkInput(bool newShrinkState)
    {
        shrink = newShrinkState;
    }

    public void RestartInput(bool newRestartState)
    {
        restart = newRestartState;

        if (restart)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    public void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
