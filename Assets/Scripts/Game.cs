using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine.Assertions;

public class Game : MonoBehaviour, IInputHandler
{
    private StateManager _stateManager;
    private InputMapPC _inputPC;

    private List<GameEntity> _gameEntities;

    public Data Data { get; private set; }
    public UI UI { get; private set; }
    public GameCamera Camera { get; private set; }
    public StateListener StateListener { get; private set; }

    public void Initialize(Data data, UI ui)
    {
        _gameEntities = new List<GameEntity>();

        // State
        StateListener = new StateListener();
        StateListener.StateChanged += StateListener_StateChanged;
        _stateManager = new StateManager(StateListener);

        // Input. TODO: Move somewhere else.
        _inputPC = gameObject.GetOrAddComponent<InputMapPC>();
        _inputPC.AddBinding(InputAction.Pause, KeyCode.Escape);
        _inputPC.AddBinding(InputAction.Left, KeyCode.A); // Kinda need to decide how to add game-specific enums here...
        _inputPC.AddBinding(InputAction.Right, KeyCode.D);
        _inputPC.AddBinding(InputAction.Jump, KeyCode.Space);

        InputManager.RegisterMap(_inputPC);
        InputManager.RegisterHandler(this);

        Data = new Data();
        UI = GetDependency<UI>();
        Camera = GetDependency<GameCamera>();

        // Level handlers
        LevelManager.LevelUnloaded += LevelManager_LevelUnloaded;
        LevelManager.LevelLoaded += LevelManager_LevelLoaded;

        InitializeEntities();
    }

    private void InitializeEntities()
    {
        foreach (var gameEntity in FindObjectsOfType<GameEntity>())
        {
            _gameEntities.Add(gameEntity);
        }

        _gameEntities.ForEach(x => x.Initialize(this));
    }

    private void CleanupEntities()
    {
        _gameEntities.ForEach(x => Destroy(x.gameObject));
        _gameEntities.Clear();
    }

    private void LevelManager_LevelLoaded(LevelManager.LevelLoadEvent obj)
    {
        Debug.LogFormat("Level {0} was loaded.", obj.SceneName);
        InitializeEntities();
    }

    private void LevelManager_LevelUnloaded(LevelManager.LevelLoadEvent obj)
    {
        Debug.LogFormat("Level {0} was unloaded.", obj.SceneName);
        CleanupEntities();
    }

    private void StateListener_StateChanged(State state)
    {
        Camera.enabled = state == State.Running;
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (action.Action == InputAction.Pause && action.Type == InputActionType.Down)
        {
            _stateManager.ToggleState();
            Debug.Log("Game is now " + _stateManager.State);
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
            LevelManager.LoadLevel("main");
    }

    T GetDependency<T>() where T : MonoBehaviour
    {
        var dependency = FindObjectOfType<T>();
        Assert.IsNotNull(dependency, string.Format("A GameObject with the {0} component must exist somewhere in the scene.", typeof(T).FullName));
        return dependency;
    }

    T GetEntity<T>() where T : GameEntity
    {
        var matchingEntity = _gameEntities.FirstOrDefault(x => x.GetType() == typeof(T));
        if (matchingEntity != null)
            return matchingEntity as T;
        return null;
    }
}
