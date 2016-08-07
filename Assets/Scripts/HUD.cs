using UnityEngine;
using System.Collections.Generic;
using Framework;
using Framework.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private HUDWorldPopups _hudPopups;

    private List<HUDWorldElement> _hudElements;

    private void Awake()
    {
        _hudElements = new List<HUDWorldElement>();
        _hudElements.Add(_hudPopups);

        LevelManager.LevelUnloaded += LevelManager_LevelUnloaded;
    }

    private void LevelManager_LevelUnloaded(LevelManager.LevelLoadEvent levelLoadEvent)
    {
        // TODO: Cleanup HUD elements on level unload.
    }
}