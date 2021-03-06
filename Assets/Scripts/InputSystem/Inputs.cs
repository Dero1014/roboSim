﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static Inputs current;

    public delegate void UserInput();
    public event UserInput CameraRotInput;
    public event UserInput FocusInput;

    private void Awake()
    {
        current = this;
    }

    void CameraRotate()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (CameraRotInput != null)
            {
                CameraRotInput();
            }
        }
    }

    void Focus()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FocusInput != null)
            {
                FocusInput();
            }
        }
    }

    void Update()
    {
        CameraRotate();
        Focus();
    }
}
