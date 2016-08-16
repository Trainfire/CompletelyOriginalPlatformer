using Framework;

class MyWorldRule : WorldRule
{
    private TokenListener _tokenListener;
    private GameController _gameController;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        var player = FindObjectOfType<Player>();

        // Find the tracking camera which causes the camera to follow a target.
        var trackingCamera = FindObjectOfType<CameraControllerTracking2D>();
        trackingCamera.SetTarget(player.gameObject);

        // Set our camera's controller so it starts tracking the player.
        var camera = EntityManager.Get<GameCamera>();
        camera.SetController(trackingCamera);

        // Hook into the TokenListener's AllCollected event.
        _tokenListener = EntityManager.Get<TokenListener>();
        if (_tokenListener != null)
            _tokenListener.AllCollected += TokenListener_AllCollected;
    }

    private void TokenListener_AllCollected(TokenListener tokenListener)
    {
        GameController.LoadMainMenu();
    }

    private void OnDestroy()
    {
        if (_tokenListener != null)
            _tokenListener.AllCollected -= TokenListener_AllCollected;
    }
}