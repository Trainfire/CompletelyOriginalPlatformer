using UnityEngine;
using Framework;
using System;

public class MainMenu : MenuBase
{
    [SerializeField] private UIMainMenu _view;

    private ZoneManager<GameZone> _zoneManager;

    protected override void OnInitialize()
    {
        _view.PlayPressed += View_PlayPressed;
        _view.QuitPressed += View_QuitPressed;
    }

    private void View_PlayPressed()
    {
        GameController.StartGame();
    }

    private void View_QuitPressed()
    {
        GameController.QuitGame();
    }

    private void OnDestroy()
    {
        _view.PlayPressed -= View_PlayPressed;
        _view.QuitPressed -= View_QuitPressed;
    }
}
