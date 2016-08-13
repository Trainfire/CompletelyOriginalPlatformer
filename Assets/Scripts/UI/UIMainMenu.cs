using UnityEngine;
using UnityEngine.UI;
using System;
using Framework;
using Framework.UI;

public class UIMainMenu : MonoBehaviour
{
    public event Action PlayPressed;
    public event Action QuitPressed;

    public UIMenuButton Play;
    public UIMenuButton Quit;

    private UIMenuButton _currentButton;

    private void Awake()
    {
        Play.Pressed += Play_Pressed;
        Quit.Pressed += Quit_Pressed;

        _currentButton = Play;
        _currentButton.Selected(true);
    }

    private void Play_Pressed(UIButton obj)
    {
        PlayPressed.InvokeSafe();
    }

    private void Quit_Pressed(UIButton obj)
    {
        QuitPressed.InvokeSafe();
    }

    public void Update()
    {
        if (_currentButton != null)
        {
            _currentButton.Button.Select();
        }
    }

    public void Up()
    {
        Move();
    }

    public void Down()
    {
        Move();
    }

    private void Move()
    {
        _currentButton.Selected(false);

        if (_currentButton == Play)
        {
            _currentButton = Quit;
            Quit.Selected(true);
        }
        else
        {
            _currentButton = Play;
            Play.Selected(true);
        }
    }
}
