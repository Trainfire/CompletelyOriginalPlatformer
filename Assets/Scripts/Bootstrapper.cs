using UnityEngine;
using Framework;

public class Bootstrapper : MonoBehaviour
{
    public void Awake()
    {
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