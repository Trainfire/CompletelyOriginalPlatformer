using UnityEngine;
using UnityEngine.Assertions;
using Framework;
using Framework.Components;
using System;

public class Player : MonoBehaviour, IInputHandler
{
    private StateListener _stateListener;
    private PlayerController _playerController;
    private ScreenShake _landEffect;
    private GameCamera _gameCamera;

    public bool InputEnabled { get; set; }

    public PlayerController Controller
    {
        get { return _playerController; }
    }

    public void Initialize(StateListener stateListener)
    {
        _stateListener = stateListener;
        _stateListener.StateChanged += OnStateChanged;
    }

    private void Start()
    {
        InputManager.RegisterHandler(this);

        _playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(_playerController, "PlayerController is missing! Make sure the player has a PlayerController attached.");

        _gameCamera = FindObjectOfType<GameCamera>();
        Assert.IsNotNull(_gameCamera, "GameCamera is missing!");

        _playerController.Landed += PlayerController_Landed;

        InputEnabled = true;
    }

    private void OnStateChanged(State state)
    {
        InputEnabled = state == State.Running;
        _playerController.enabled = state == State.Running;
    }

    private void PlayerController_Landed(PlayerController.LandEventArgs landEvent)
    {
        // I don't like this dependency, tbh...
        if (_gameCamera == null)
            return;

        if (Mathf.Abs(landEvent.Velocity) > 10f)
        {
            _landEffect = _gameCamera.AddScreenEffect<ScreenShake>();
            _landEffect.Amplitude = Mathf.Abs(landEvent.Velocity) / 100f;
            _landEffect.Duration = 0.5f;
            _landEffect.Frequency = 0.01f;
            _landEffect.Activate();
        }
    }

    private void OnDestroy()
    {
        InputManager.UnregisterHandler(this);

        if (_playerController != null)
            _playerController.Landed -= PlayerController_Landed;
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (!InputEnabled)
            return;

        _playerController.HandleInput(action);
    }
}
