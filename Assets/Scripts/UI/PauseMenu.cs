using UnityEngine;
using Framework;
using Framework.UI;

public class PauseMenu : MenuBase, IInputHandler
{
    private UIButton _resume;
    private UIButton _quit;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        gameObject.SetActive(false);

        _resume = Buttons.Add("Resume");
        _resume.Pressed += View_ResumePressed;

        _quit = Buttons.Add("Quit");
        _quit.Pressed += View_QuitPressed;
    }

    protected override void OnStateChanged(State state)
    {
        base.OnStateChanged(state);
        gameObject.SetActive(state == State.Paused);
    }

    private void View_ResumePressed(UIButton sender)
    {
        Game.Controller.Resume();
    }

    private void View_QuitPressed(UIButton sender)
    {
        Game.Controller.LoadMainMenu();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _resume.Pressed -= View_ResumePressed;
        _quit.Pressed -= View_QuitPressed;
    }
}
