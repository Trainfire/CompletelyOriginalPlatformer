using UnityEngine;
using Framework;
using System.Collections;
using System;

public class Player : MonoBehaviour, IInputHandler
{
    public bool InputEnabled { get; set; }

    public void Awake()
    {
        InputEnabled = true;
    }

    public void HandleInput(InputActionEvent action)
    {
        if (!InputEnabled)
            return;

        Debug.LogFormat("Received input: {0}", action.Action);
    }
}
