using UnityEngine;
using Framework;
using System;

public class PauseMenu : GameControllerDependant
{
    [SerializeField] private UIPauseMenu _view;

    protected override void OnInitialize()
    {
        GameController.Game.StateListener.StateChanged += StateListener_OnStateChanged;

        _view.gameObject.SetActive(false);
        _view.ResumePressed += View_ResumePressed;
        _view.QuitPressed += View_QuitPressed;
    }

    private void StateListener_OnStateChanged(State state)
    {
        _view.gameObject.SetActive(state == State.Paused);
    }

    private void View_ResumePressed()
    {
        GameController.Resume();
    }

    private void View_QuitPressed()
    {
        GameController.LoadMainMenu();
    }

    private void OnDestroy()
    {
        _view.ResumePressed -= View_ResumePressed;
        _view.QuitPressed -= View_QuitPressed;

        // Buh?
        if (GameController != null)
            GameController.Game.StateListener.StateChanged -= StateListener_OnStateChanged;
    }
}