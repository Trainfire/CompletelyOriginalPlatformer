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
    public ZoneManager<GameZone> ZoneManager { get; private set; }
    public ZoneListener<GameZone> ZoneListener { get; private set; }
    public StateManager StateManager { get; private set; }

    public void Initialize(Data data, UI ui)
    {
        // State
        StateListener = new StateListener();
        StateListener.StateChanged += StateListener_StateChanged;
        StateManager = new StateManager(StateListener);

        // Zone
        ZoneListener = new ZoneListener<GameZone>();
        ZoneListener.ZoneChanged += ZoneListener_ZoneChanged;
        ZoneManager = new ZoneManager<GameZone>(ZoneListener);

        // Input. TODO: Move somewhere else.
        //_inputPC = gameObject.GetOrAddComponent<InputMapPC>();
        //_inputPC.AddBinding(InputAction.Pause, KeyCode.Escape);
        //_inputPC.AddBinding(InputAction.Left, KeyCode.A); // Kinda need to decide how to add game-specific enums here...
        //_inputPC.AddBinding(InputAction.Right, KeyCode.D);
        //_inputPC.AddBinding(InputAction.Jump, KeyCode.Space);

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
            LevelManager.LoadLevel("main");
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
