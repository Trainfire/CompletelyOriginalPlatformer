using UnityEngine;
using Framework;
using System;

public class MainMenu : GameEntity
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
        Game.Controller.StartGame();
    }

    private void View_QuitPressed()
    {
        Game.Controller.QuitGame();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _view.PlayPressed -= View_PlayPressed;
        _view.QuitPressed -= View_QuitPressed;
    }
}
