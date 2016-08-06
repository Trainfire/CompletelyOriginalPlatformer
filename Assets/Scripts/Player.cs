using UnityEngine;
using UnityEngine.Assertions;
using Framework;
using Framework.Components;
using System;

public class Player : GameEntity, IInputHandler, IStateHandler
{
    public bool InputEnabled { get; set; }

    private PlayerController _playerController;
    private ScreenShake _landEffect;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(_playerController, "PlayerController is missing! Make sure the player has a PlayerController attached.");

        _playerController.Landed += PlayerController_Landed;

        InputManager.RegisterHandler(this);
        InputEnabled = true;

        Game.StateListener.StateChanged += StateListener_StateChanged;
    }

    private void StateListener_StateChanged(State state)
    {
        InputEnabled = state == State.Running;
        _playerController.enabled = state == State.Running;
    }

    private void PlayerController_Landed(PlayerController.LandEventArgs landEvent)
    {
        if (Mathf.Abs(landEvent.Velocity) > 10f)
        {
            _landEffect = Game.Camera.AddScreenEffect<ScreenShake>();
            _landEffect.Amplitude = Mathf.Abs(landEvent.Velocity) / 100f;
            _landEffect.Duration = 0.5f;
            _landEffect.Frequency = 0.01f;
            _landEffect.Activate();
        }
    }

    protected override void OnDestroy()
    {
        InputManager.UnregisterHandler(this);
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (!InputEnabled)
            return;

        _playerController.HandleInput(action);
    }

    void IStateHandler.OnStateChanged(State state)
    {
        InputEnabled = state == State.Running;
        _playerController.Enabled = state == State.Running;
    }
}
