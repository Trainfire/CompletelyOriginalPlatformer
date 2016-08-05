using UnityEngine;
using UnityEngine.Assertions;
using Framework;
using Framework.Components;

public class Player : MonoBehaviour, IInputHandler
{
    public bool InputEnabled { get; set; }

    private Game _game;
    private PlayerController _playerController;
    private ScreenShake _landEffect;

    public void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        Assert.IsNotNull(_playerController, "PlayerController is missing! Make sure the player has a PlayerController attached.");

        _playerController.Landed += PlayerController_Landed;
    }

    public void Initialize(Game game)
    {
        _game = game;
    }

    private void PlayerController_Landed(PlayerController.LandEventArgs landEvent)
    {
        if (Mathf.Abs(landEvent.Velocity) > 10f)
        {
            _landEffect = _game.Camera.AddScreenEffect<ScreenShake>();
            _landEffect.Amplitude = Mathf.Abs(landEvent.Velocity) / 100f;
            _landEffect.Duration = 0.5f;
            _landEffect.Frequency = 0.01f;
            _landEffect.Activate();
        }
    }

    public void HandleInput(InputActionEvent action)
    {
        if (!InputEnabled)
            return;

        _playerController.HandleInput(action);
    }
}
