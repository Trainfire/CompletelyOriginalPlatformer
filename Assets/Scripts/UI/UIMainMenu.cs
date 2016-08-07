using UnityEngine;
using UnityEngine.UI;
using System;

public class UIMainMenu : MonoBehaviour
{
    public event Action PlayPressed;
    public event Action QuitPressed;

    public Button Play;
    public Button Quit;

    private void Awake()
    {
        Play.onClick.AddListener(OnPlay);
        Quit.onClick.AddListener(OnQuit);
    }

    private void OnPlay()
    {
        if (PlayPressed != null)
            PlayPressed();
    }

    private void OnQuit()
    {
        if (QuitPressed != null)
            QuitPressed();
    }
}