﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Framework;

/// <summary>
/// Exposes parts of the Game. Used only by the UI.
/// </summary>
public class GameController : IInputHandler
{
    private StateManager _stateManager;
    private ZoneManager<GameZone> _zoneManager;

    public Game Game { get; private set; }

    public GameController(Game game, StateManager stateManager, ZoneManager<GameZone> zoneManager)
    {
        Game = game;
        _stateManager = stateManager;
        _zoneManager = zoneManager;
        _zoneManager.Listener.ZoneChanged += Listener_ZoneChanged;

        InputManager.RegisterHandler(this);
    }

    private void Listener_ZoneChanged(GameZone gameZone)
    {
        _stateManager.SetState(State.Running);

        var menus = GameObject.FindObjectsOfType<MenuBase>();
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].Initialize(this);
        }
    }

    public void StartGame()
    {
        LoadLevel("level");
    }

    public void Resume()
    {
        _stateManager.SetState(State.Running);
    }

    public void Pause()
    {
        _stateManager.SetState(State.Paused);
    }

    public void QuitGame()
    {
        throw new NotImplementedException();
    }

    public void LoadMainMenu()
    {
        _zoneManager.SetZone(GameZone.MainMenu, "MainMenu");
    }

    public void LoadLevel(string sceneName)
    {
        _zoneManager.SetZone(GameZone.InGame, "InGame", sceneName);
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (action.Action == InputAction.Pause && action.Type == InputActionType.Down)
        {
            _stateManager.ToggleState();
            Debug.Log("Game is now " + _stateManager.State);
        }
    }
}


public class ZoneChanger : MonoBehaviour
{
    public void Initialize(ZoneManager<GameZone> zoneManager)
    {

    }
}