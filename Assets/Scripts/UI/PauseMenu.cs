using UnityEngine;
using Framework;
using System;

public class PauseMenu : GameEntity
{
    [SerializeField] private UIPauseMenu _view;

    protected override void OnInitialize()
    {
        _view.gameObject.SetActive(false);
        _view.ResumePressed += View_ResumePressed;
        _view.QuitPressed += View_QuitPressed;
    }

    protected override void OnStateChanged(State state)
    {
        base.OnStateChanged(state);
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
    }
}
