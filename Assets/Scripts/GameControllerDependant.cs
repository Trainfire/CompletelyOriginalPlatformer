using UnityEngine;
using Framework;
using System;

public class GameControllerDependant : MonoBehaviourEx
{
    protected GameController GameController { get; private set; }

    public void Initialize(GameController gameController)
    {
        GameController = gameController;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }
}