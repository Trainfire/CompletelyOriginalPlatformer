using UnityEngine;
using UnityEngine.SceneManagement;
using Framework;

public class MyGame : Game
{
    protected override void OnInitialize(params string[] args)
    {
        base.OnInitialize(args);

        // Set loading scene.
        SceneLoader.LoadingScene = "Loader";

        // Input
        var inputPC = gameObject.GetOrAddComponent<InputMapPC>();
        inputPC.AddBinding(InputMap.Up, KeyCode.W);
        inputPC.AddBinding(InputMap.Down, KeyCode.S);
        inputPC.AddBinding(InputMap.Left, KeyCode.A);
        inputPC.AddBinding(InputMap.Right, KeyCode.D);
        inputPC.AddBinding(InputMap.Pause, KeyCode.Escape);
        inputPC.AddBinding(MyGameInputActions.Jump, KeyCode.Space);
        inputPC.AddBinding(MyGameInputActions.Interact, KeyCode.E);

        // Register Map(s)
        InputManager.RegisterMap(inputPC);

        // Determines where to go first.
        if (args != null && args.Length != 0)
        {
            Controller.LoadLevel(args[0]);
        }
        else
        {
            Controller.LoadMainMenu();
        }
    }
}

public class MyGameInputActions
{
    public const string Jump = "Jump";
    public const string Interact = "Interact";
}

public static class InputExtensions
{
    public static bool IsInteract(this InputActionEvent action)
    {
        return action.Action == MyGameInputActions.Interact && action.Type == InputActionType.Down;
    }

    public static bool IsHeldInteract(this InputActionEvent action)
    {
        return action.Action == MyGameInputActions.Interact && action.Type == InputActionType.Held;
    }
}
