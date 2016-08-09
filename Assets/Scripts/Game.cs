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

    private GameZoneManager _zoneManager;
    public ZoneListener<GameZone> ZoneListener
    {
        get { return _zoneManager.Listener; }
    }

    public GameCamera Camera { get; private set; }

    public void Initialize(UI ui, GameCamera gameCamera)
    {
        _stateManager = new StateManager();
        _zoneManager = gameObject.AddComponent<GameZoneManager>();

        new GameEntityManager(this);

        Camera = gameCamera;
        Camera.Initialize(this);

        // Allows the UI to control the game. IE, Resuming, Pausing, Load Level, etc.
        var gameController = new GameController(this, _stateManager, _zoneManager);

        // Load main menu.
        gameController.LoadMainMenu();
    }
}

// Unity can't instantiate types with generic parameters so we have to do this...
class GameZoneManager : ZoneManager<GameZone> { }