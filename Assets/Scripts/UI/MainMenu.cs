using UnityEngine;
using Framework;
using System;

public class MainMenu : MonoBehaviour, IInputHandler
{
    [SerializeField] private UIMainMenu _view;

    private ZoneManager<GameZone> _zoneManager;

    public void Initialize(ZoneManager<GameZone> zoneManager)
    {
        _zoneManager = zoneManager;

        _view.PlayPressed += View_PlayPressed;
        _view.QuitPressed += View_QuitPressed;
    }

    private void View_PlayPressed()
    {
        _zoneManager.SetZone(GameZone.InGame);
    }

    private void View_QuitPressed()
    {
        // TODO.
    }

    private void OnDestroy()
    {
        _view.PlayPressed -= View_PlayPressed;
        _view.QuitPressed -= View_QuitPressed;
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        // TODO.
    }
}