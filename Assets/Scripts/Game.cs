using UnityEngine;
using System;
using System.Collections.Generic;
using Framework;
using UnityEngine.Assertions;

public class Game : MonoBehaviour, IInputHandler, IStateHandler
{
    private StateManager _stateManager;
    private InputMapPC _inputPC;

    private List<GameEntity> _gameEntities;
    private List<IInputHandler> _inputHandlers;
    private List<IStateHandler> _stateHandlers;

    public Data Data { get; private set; }
    public UI UI { get; private set; }
    public TrackingCamera2D Camera { get; private set; }
    public Player Player { get; private set; }
    public World World { get; private set; }

    public void Initialize(Data data, UI ui)
    {
        // State
        _stateManager = new StateManager();
        _stateManager.RegisterListener(this);

        // Input
        _inputPC = gameObject.GetOrAddComponent<InputMapPC>();
        _inputPC.AddBinding(InputAction.Pause, KeyCode.Escape);
        _inputPC.AddBinding(InputAction.Left, KeyCode.A); // Kinda need to decide how to add game-specific enums here...
        _inputPC.AddBinding(InputAction.Right, KeyCode.D);
        _inputPC.AddBinding(InputAction.Jump, KeyCode.Space);

        InputManager.RegisterMap(_inputPC);
        InputManager.RegisterHandler(this);

        // Setup
        _gameEntities = new List<GameEntity>();
        _inputHandlers = new List<IInputHandler>();
        _stateHandlers = new List<IStateHandler>();

        Data = new Data();
        UI = GetDependency<UI>();
        Player = GetDependency<Player>();
        World = GetDependency<World>();
        Camera = GetDependency<TrackingCamera2D>();

        foreach (var gameEntity in _gameEntities)
        {
            var inputHandler = gameEntity as IInputHandler;
            if (inputHandler != null)
                _inputHandlers.Add(inputHandler);

            var stateHandler = gameEntity as IStateHandler;
            if (stateHandler != null)
                _stateHandlers.Add(stateHandler);
        }

        // Init
        Player.InputEnabled = true;
        _gameEntities.ForEach(x => x.Initialize(this));
    }

    T GetDependency<T>() where T : GameEntity
    {
        var dependency = FindObjectOfType<T>();
        Assert.IsNotNull(dependency, string.Format("A GameObject with the {0} component must exist somewhere in the scene.", typeof(T).FullName));
        _gameEntities.Add(dependency);
        return dependency;
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (action.Action == InputAction.Pause && action.Type == InputActionType.Down)
            _stateManager.ToggleState();

        _inputHandlers.ForEach(x => x.HandleInput(action));

    }

    void IStateHandler.OnStateChanged(State state)
    {
        _stateHandlers.ForEach(x => x.OnStateChanged(state));
    }
}
