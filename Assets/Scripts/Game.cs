using UnityEngine;
using System;
using System.Collections.Generic;
using Framework;
using UnityEngine.Assertions;

public class Game : MonoBehaviour, IInputHandler, IStateListener
{
    private Data _data;
    private UI _ui;

    private StateManager _stateManager;
    private InputMapPC _inputPC;
    private Player _player;
    private World _world;

    public void Initialize(Data data, UI ui)
    {
        _data = data;
        _ui = ui;

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
        _player = FindObjectOfType<Player>();
        Assert.IsNotNull(_player, "A GameObject with the Player component must exist somewhere in the scene.");
        _player.InputEnabled = true;

        _world = FindObjectOfType<World>();
        Assert.IsNotNull(_world, "A GameObject with the World component must exist somewhere in the scene.");
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        _ui.HandleInput(action);
        _player.HandleInput(action);

        if (action.Action == InputAction.Pause && action.Type == InputActionType.Down)
            _stateManager.ToggleState();
    }

    void IStateListener.OnStateChanged(State state)
    {
        _player.InputEnabled = state == State.Running;
        _player.enabled = state == State.Running;
        _world.enabled = state == State.Running;
    }
}
