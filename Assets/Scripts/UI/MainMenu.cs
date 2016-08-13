using UnityEngine;
using Framework;
using Framework.UI;

public class MainMenu : MenuBase, IInputHandler
{
    private UIButton _play;
    private UIButton _quit;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _play = Buttons.Add("Play");
        _quit = Buttons.Add("Quit");

        _play.Pressed += View_PlayPressed;
        _quit.Pressed += View_QuitPressed;
    }

    private void View_PlayPressed(UIButton sender)
    {
        Game.Controller.StartGame();
    }

    private void View_QuitPressed(UIButton sender)
    {
        Game.Controller.QuitGame();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _play.Pressed -= View_PlayPressed;
        _quit.Pressed -= View_QuitPressed;
    }
}
