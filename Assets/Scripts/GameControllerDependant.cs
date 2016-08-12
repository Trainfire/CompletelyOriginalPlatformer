using UnityEngine;
using Framework;
using System;

public class GameControllerDependant : MonoBehaviourEx
{
    protected GameController GameController { get; private set; }

    public int InstanceID
    {
        get { return GetInstanceID(); }
    }

    public void Initialize(GameController gameController)
    {
        GameController = gameController;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }
}
