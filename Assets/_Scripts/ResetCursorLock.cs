using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCursorLock : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
