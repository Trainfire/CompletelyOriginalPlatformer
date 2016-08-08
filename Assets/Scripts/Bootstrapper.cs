using UnityEngine;
using Framework;

public class Bootstrapper : MonoBehaviour
{
    public void Awake()
    {
        var existingGame = FindObjectOfType<Bootstrapper>();
        if (existingGame != null && existingGame != this)
        {
            Debug.LogWarning("A GameObject with the Game component already exists in the scene. Destroying it...");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        // Relay
        gameObject.GetOrAddComponent<MonoEventRelay>();

        // Input
        var _inputPC = ObjectEx.FindObjectOfType<InputMapPC>();
        InputManager.RegisterMap(_inputPC);

        var ui = ObjectEx.FindObjectOfType<UI>();
        var camera = ObjectEx.FindObjectOfType<GameCamera>();

        // Inject dependencies
        var game = gameObject.AddComponent<Game>();
        game.Initialize(ui, camera);
    }
}