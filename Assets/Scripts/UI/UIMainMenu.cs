using UnityEngine;
using UnityEngine.UI;
using System;
using Framework;
using Framework.UI;

public class UIMainMenu : MonoBehaviour
{
    public event Action PlayPressed;
    public event Action QuitPressed;

    [SerializeField] private ButtonList _buttonList;
    public ButtonList ButtonList
    {
        get { return _buttonList; }
    }

    private UIMenuButton _play;
    private UIMenuButton _quit;

    private void Start()
    {
        _play = _buttonList.Add("Play") as UIMenuButton;
        _quit = _buttonList.Add("Quit") as UIMenuButton;

        _play.Pressed += Play_Pressed;
        _quit.Pressed += Quit_Pressed;
    }

    private void Play_Pressed(UIButton obj)
    {
        PlayPressed.InvokeSafe();
    }

    private void Quit_Pressed(UIButton obj)
    {
        QuitPressed.InvokeSafe();
    }

    private void OnDestroy()
    {
        _play.Pressed -= Play_Pressed;
        _quit.Pressed -= Quit_Pressed;
    }
}
