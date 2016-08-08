using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections;
using Framework;
using System;

public class UI : MonoBehaviourEx, IInputHandler
{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private HUD _hud;

    private GameController _gameController;

    public void Initialize(GameController gameController)
    {
        _gameController = gameController;

        Assert.IsNotNull(_hud, "HUD is null and shouldn't be.");
        Assert.IsNotNull(_pauseMenu, "PauseMenu is null and shouldn't be.");

        _pauseMenu.Initialize(_gameController);
    }

    public void HandleInput(InputActionEvent action)
    {
        // TODO.
    }
}
