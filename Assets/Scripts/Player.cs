using UnityEngine;
using UnityEngine.Assertions;
using Framework;
using Framework.Components;
using System;

public class Player : GameEntity, IInputHandler
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
    }

    protected override void OnStateChanged(State state)
    {
        base.OnStateChanged(state);
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
        base.OnDestroy();
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
