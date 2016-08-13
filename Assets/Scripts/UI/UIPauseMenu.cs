using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPauseMenu : MonoBehaviour
{
    public event Action ResumePressed;
    public event Action QuitPressed;

    public Button Resume;
    public Button Quit;

    private void Awake()
    {
        Resume.onClick.AddListener(OnResume);
        Quit.onClick.AddListener(OnQuit);
    }

    private void OnResume()
    {
        if (ResumePressed != null)
            ResumePressed();
    }

    private void OnQuit()
    {
        if (QuitPressed != null)
            QuitPressed();
    }
}
