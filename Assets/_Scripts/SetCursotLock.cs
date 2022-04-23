using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursotLock : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
