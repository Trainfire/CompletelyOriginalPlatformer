using UnityEngine;
using System;
using System.Collections;
using Framework;

public class Token : GameEntity
{
    public event Action<Token> Collected;

    private void OnTriggerEnter2D()
    {
        Collected.InvokeSafe(this);
    }
}
