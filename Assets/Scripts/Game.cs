using UnityEngine;
using UnityEngine.SceneManagement;
using Framework;

public class Game : MonoBehaviour
{
    private ConsoleController _console;
    private GameEntityManager _gameEntityManager;
    private GameController _gameController;

    private StateManager _stateManager;
    public StateListener StateListener
    {
        get { return _stateManager.Listener; }
    }

    private GameZoneManager _zoneManager;
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
            Debug.LogWarning("Game has already been initialized. This should not happen!");
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

        // Input
        var _inputPC = ObjectEx.FindObjectOfType<InputMapPC>();
        InputManager.RegisterMap(_inputPC);

        _stateManager = new StateManager();

        _zoneManager = gameObject.AddComponent<GameZoneManager>();
        _zoneManager.LoadingScene = "loader";
        _zoneManager.Listener.ZoneChanging += Listener_ZoneChanging;
        _zoneManager.Listener.ZoneChanged += Listener_ZoneChanged;

        _gameEntityManager = new GameEntityManager(this);

        // Allows the control of the game. IE, Resuming, Pausing, Load Level, etc.
        _gameController = new GameController(this, _stateManager, _zoneManager);

        // Determine which zone to go to. Not the cleanest implementation but w/e.
        if (sceneName != null)
        {
            _gameController.LoadLevel(sceneName);
        }
        else
        {
            _gameController.LoadMainMenu();
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

        var controllerDependants = GameObject.FindObjectsOfType<GameControllerDependant>();
        for (int i = 0; i < controllerDependants.Length; i++)
        {
            controllerDependants[i].Initialize(_gameController);
        }
    }
}

// Unity can't instantiate types with generic parameters so we have to do this...
class GameZoneManager : ZoneManager<GameZone> { }
