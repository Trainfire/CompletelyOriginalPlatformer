using UnityEngine;
using UnityEngine.SceneManagement;
using Framework;

public class Game : MonoBehaviour
{
    private ConsoleController _console;
    private GameEntityManager _gameEntityManager;
    private GameController _controller;
    public GameController Controller
    {
        get { return _controller; }
    }

    private StateManager _stateManager;
    public StateListener StateListener
    {
        get { return _stateManager.Listener; }
    }

    private ZoneManager<GameZone> _zoneManager;
    public ZoneListener<GameZone> ZoneListener
    {
        get { return _zoneManager.Listener; }
    }

    public bool Initialized { get; private set; }
    public GameCamera Camera { get; private set; }

    public void Initialize(string sceneName = null)
    {
        if (Initialized)
        {
            Debug.LogError("Game has already been initialized. This should not happen!");
            return;
        }

        Initialized = true;

        // Console
        var consoleView = FindObjectOfType<ConsoleView>();
        if (consoleView == null)
        {
            Debug.LogError("Failed to find a ConsoleView! The Console will be unavailable!");
        }
        else
        {
            _console = new ConsoleController();
            consoleView.SetConsole(_console);
        }

        // Relay
        gameObject.GetOrAddComponent<MonoEventRelay>();

        // Input Bindings
        var inputPC = gameObject.GetOrAddComponent<InputMapPC>();
        inputPC.AddBinding(InputMap.Left, KeyCode.A);
        inputPC.AddBinding(InputMap.Right, KeyCode.D);
        inputPC.AddBinding(InputMap.Pause, KeyCode.Escape);
        inputPC.AddBinding(GameInputActions.Jump, KeyCode.Space);

        // Register Input Map(s)
        InputManager.RegisterMap(inputPC);

        // State
        _stateManager = new StateManager();

        // Scene Loader
        var sceneLoader = gameObject.GetOrAddComponent<SceneLoader>();
        sceneLoader.LoadingScene = "Loader";

        // Zone
        _zoneManager = new ZoneManager<GameZone>(sceneLoader);
        _zoneManager.Listener.ZoneChanging += Listener_ZoneChanging;
        _zoneManager.Listener.ZoneChanged += Listener_ZoneChanged;

        _gameEntityManager = new GameEntityManager(this);

        // Allows the control of the game, such as level loading, resuming and pausing the game.
        _controller = new GameController(this, _stateManager, _zoneManager);

        // Determines where to go first.
        if (sceneName != null)
        {
            _controller.LoadLevel(sceneName);
        }
        else
        {
            _controller.LoadMainMenu();
        }
    }

    private void Listener_ZoneChanging()
    {
        _gameEntityManager.Cleanup();
    }

    private void Listener_ZoneChanged(GameZone gameZone)
    {
        _stateManager.SetState(State.Running);

        var gameCamera = GameObject.FindObjectOfType<GameCamera>();
        Camera = gameCamera;
        if (Camera != null)
            Camera.Initialize(this);

        _gameEntityManager.LoadEntities();
    }
}

public class GameInputActions
{
    public const string Jump = "Jump";
}
