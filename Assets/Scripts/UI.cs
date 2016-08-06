using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections;
using Framework;
using System;

public class UI : MonoBehaviourEx, IInputHandler
{
    [SerializeField] private HUD _hud;

    public HUD HUD
    {
        get { return _hud; }
    }

    public void Awake()
    {
        Assert.IsNotNull(HUD, "HUD is null and shouldn't be.");
    }

    public void HandleInput(InputActionEvent action)
    {
        // TODO.
    }
}
