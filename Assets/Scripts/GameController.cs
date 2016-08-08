using UnityEngine;
using System;
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

        LevelManager.LevelLoaded += LevelManager_LevelLoaded;

        InputManager.RegisterHandler(this);
    }

    private void LevelManager_LevelLoaded(LevelManager.LevelLoadEvent obj)
    {
        if (obj.SceneName == "mainmenu")
        {
            // Game was probably paused when returning to menu. So set it back to Running.
            _stateManager.SetState(State.Running);

            _zoneManager.SetZone(GameZone.MainMenu);

            var mainMenu = GameObject.FindObjectOfType<MainMenu>();
            if (mainMenu == null)
            {
                Debug.LogError("Failed to find the MainMenu.");
            }
            else
            {
                mainMenu.Initialize(this);
            }
        }
        else
        {
            _zoneManager.SetZone(GameZone.InGame);
        }
    }

    public void StartGame()
    {
        _zoneManager.SetZone(GameZone.InGame);
        LevelManager.LoadLevel("level");
    }

    public void Resume()
    {
        _stateManager.SetState(State.Running);
    }

    public void Pause()
    {
        _stateManager.SetState(State.Paused);
    }

    public void QuitToMainMenu()
    {
        LevelManager.LoadLevel("mainmenu");
    }

    public void QuitGame()
    {
        throw new NotImplementedException();
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