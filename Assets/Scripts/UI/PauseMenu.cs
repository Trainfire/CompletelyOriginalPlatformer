using UnityEngine;
using Framework;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIPauseMenu _view;

    private StateManager _stateManager;
    private StateListener _stateListener;
    private ZoneManager<GameZone> _zoneManager;

    public void Initialize(StateManager stateManager, StateListener stateListener, ZoneManager<GameZone> zoneManager)
    {
        _stateManager = stateManager;
        _stateListener = stateListener;
        _stateListener.StateChanged += StateListener_OnStateChanged;
        _zoneManager = zoneManager;

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
        _stateManager.SetState(State.Running);
    }

    private void View_QuitPressed()
    {
        _zoneManager.SetZone(GameZone.MainMenu);
    }

    private void OnDestroy()
    {
        _view.ResumePressed -= View_ResumePressed;
        _view.QuitPressed -= View_QuitPressed;

        if (_stateListener != null)
            _stateListener.StateChanged -= StateListener_OnStateChanged;
    }
}