using UnityEngine;
using Framework;
using System;

public class MainMenu : GameEntity, IInputHandler
{
    [SerializeField] private UIMainMenu _view;

    protected override void OnInitialize()
    {
        InputManager.RegisterHandler(this);

        _view.PlayPressed += View_PlayPressed;
        _view.QuitPressed += View_QuitPressed;
    }

    private void View_PlayPressed()
    {
        Game.Controller.StartGame();
    }

    private void View_QuitPressed()
    {
        Game.Controller.QuitGame();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        InputManager.UnregisterHandler(this);

        _view.PlayPressed -= View_PlayPressed;
        _view.QuitPressed -= View_QuitPressed;
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (action.Type == InputActionType.Down)
        {
            if (action.Action == InputMap.Up)
                _view.Up();

            if (action.Action == InputMap.Down)
                _view.Down();
        }
    }
}
