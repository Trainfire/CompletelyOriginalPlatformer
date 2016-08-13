using UnityEngine;
using Framework;
using System;

public class PauseMenu : GameEntity
{
    [SerializeField] private UIPauseMenu _view;

    protected override void OnInitialize()
    {
        Game.StateListener.StateChanged += StateListener_OnStateChanged;

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
        Game.Controller.Resume();
    }

    private void View_QuitPressed()
    {
        Game.Controller.LoadMainMenu();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _view.ResumePressed -= View_ResumePressed;
        _view.QuitPressed -= View_QuitPressed;

        // Buh?
        if (Game != null)
            Game.StateListener.StateChanged -= StateListener_OnStateChanged;
    }
}
