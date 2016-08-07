using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine.Assertions;

public class Game : MonoBehaviour, IInputHandler
{
    private InputMapPC _inputPC;

    private GameEntityManager _gameEntityManager;

    public Data Data { get; private set; }
    public UI UI { get; private set; }
    public GameCamera Camera { get; private set; }
    public StateListener StateListener { get; private set; }
    public StateManager StateManager { get; private set; }
    public ZoneManager<GameZone> ZoneManager { get; private set; }
    public ZoneListener<GameZone> ZoneListener { get; private set; }
    public MonoEventRelay MonoEventRelay { get; private set; }

    public void Awake()
    {
        var existingGame = FindObjectOfType<Game>();
        if (existingGame != null && existingGame != this)
        {
            Debug.LogError("A GameObject with the Game component already exists in the scene. Destroying it...");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        // Relay
        MonoEventRelay = gameObject.GetOrAddComponent<MonoEventRelay>();

        // State
        StateListener = new StateListener();
        StateListener.StateChanged += StateListener_StateChanged;
        StateManager = new StateManager(StateListener);

        // Zone
        ZoneListener = new ZoneListener<GameZone>();
        ZoneListener.ZoneChanged += ZoneListener_ZoneChanged;
        ZoneManager = new ZoneManager<GameZone>(ZoneListener);

        // Level
        LevelManager.LevelLoaded += LevelManager_LevelLoaded;

        // Input
        _inputPC = ObjectEx.FindObjectOfType<InputMapPC>();
        InputManager.RegisterMap(_inputPC);
        InputManager.RegisterHandler(this);

        Data = new Data();
        UI = ObjectEx.FindObjectOfType<UI>();
        Camera = ObjectEx.FindObjectOfType<GameCamera>();

        // Inject dependencies
        _gameEntityManager = new GameEntityManager(this);
        UI.Initialize(this);
    }

    private void ZoneListener_ZoneChanged(GameZone zone)
    {
        if (zone == GameZone.MainMenu)
        {
            StateManager.SetState(State.Running);
            LevelManager.LoadLevel("mainmenu");
        }
        else
        {
            LevelManager.LoadLevel("level");
        }
    }

    private void LevelManager_LevelLoaded(LevelManager.LevelLoadEvent levelLoadEvent)
    {
        if (ZoneManager.Zone == GameZone.MainMenu)
        {
            var mainMenu = ObjectEx.FindObjectOfType<MainMenu>();
            mainMenu.Initialize(ZoneManager);
        }
    }

    private void StateListener_StateChanged(State state)
    {
        Camera.enabled = state == State.Running;
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (action.Action == InputAction.Pause && action.Type == InputActionType.Down)
        {
            StateManager.ToggleState();
            Debug.Log("Game is now " + StateManager.State);
        }
    }
}
