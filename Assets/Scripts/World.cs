using UnityEngine;
using UnityEngine.Assertions;
using Framework;

class World : GameEntity
{
    private CameraControllerTracking2D _trackingCamera;
    private TokenListener _tokenListener;

    protected override void OnInitialize()
    {
        var player = FindObjectOfType<Player>();

        // Find the tracking camera which causes the camera to follow a target.
        _trackingCamera = FindObjectOfType<CameraControllerTracking2D>();
        _trackingCamera.SetTarget(player.gameObject);

        // Set our camera's controller so it starts tracking the player.
        Game.Camera.SetController(_trackingCamera);

        _tokenListener = FindObjectOfType<TokenListener>();
        _tokenListener.AllCollected += TokenListener_AllCollected;
    }

    private void TokenListener_AllCollected(TokenListener obj)
    {
        Game.Controller.LoadMainMenu();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_tokenListener != null)
            _tokenListener.AllCollected -= TokenListener_AllCollected;
    }
}
