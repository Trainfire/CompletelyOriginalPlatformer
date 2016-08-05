using UnityEngine;
using Framework;
using System.Collections;
using System;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IInputHandler
{
    public bool InputEnabled { get; set; }

    private PlayerController _playerController;

    public void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(_playerController, "PlayerController is missing! Make sure the player has a PlayerController attached.");
    }

    public void HandleInput(InputActionEvent action)
    {
        if (!InputEnabled)
            return;

        _playerController.HandleInput(action);
    }
}
