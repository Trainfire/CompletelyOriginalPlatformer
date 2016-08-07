using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections;
using Framework;
using System;

public class UI : MonoBehaviourEx, IInputHandler, IGameEntity
{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private HUD _hud;

    private Game _game;

    public HUD HUD
    {
        get { return _hud; }
    }

    public void Initialize(Game game)
    {
        _game = game;

        Assert.IsNotNull(HUD, "HUD is null and shouldn't be.");
        Assert.IsNotNull(_pauseMenu, "PauseMenu is null and shouldn't be.");

        game.ZoneListener.ZoneChanged += ZoneListener_ZoneChanged;

        _pauseMenu.Initialize(game.StateManager, game.StateListener, game.ZoneManager);
    }

    private void ZoneListener_ZoneChanged(GameZone gameZone)
    {
        if (gameZone == GameZone.MainMenu)
            _pauseMenu.gameObject.SetActive(false);
    }

    public void HandleInput(InputActionEvent action)
    {
        // TODO.
    }
}
