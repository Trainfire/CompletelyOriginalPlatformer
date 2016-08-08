using UnityEngine;
using System;
using Framework;

public class Game : MonoBehaviour
{
    private StateManager _stateManager;
    public StateListener StateListener
    {
        get { return _stateManager.Listener; }
    }

    private ZoneManager<GameZone> _zoneManager;
    public ZoneListener<GameZone> ZoneListener
    {
        get { return _zoneManager.Listener; }
    }

    public GameCamera Camera { get; private set; }

    public void Initialize(UI ui, GameCamera gameCamera)
    {
        _stateManager = new StateManager();
        _zoneManager = new ZoneManager<GameZone>();

        new GameEntityManager(this);

        Camera = gameCamera;
        Camera.Initialize(this);

        // Allows the UI to control the game. IE, Resuming, Pausing, Load Level, etc.
        var gameController = new GameController(this, _stateManager, _zoneManager);

        // Initialize UI.
        ui.Initialize(gameController);

        // Load main menu.
        gameController.QuitToMainMenu();
    }
}