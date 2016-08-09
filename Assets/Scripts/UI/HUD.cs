using UnityEngine;
using System.Collections.Generic;
using Framework;
using Framework.UI;

public class HUD : MenuBase
{
    [SerializeField] private HUDWorldPopups _hudPopups;

    private List<HUDWorldElement> _hudElements;

    protected override void OnInitialize()
    {
        GameController.Game.ZoneListener.ZoneChanging += ZoneListener_ZoneChanging;

        _hudElements = new List<HUDWorldElement>();
        _hudElements.Add(_hudPopups);
    }

    private void ZoneListener_ZoneChanging()
    {
        // TODO: Cleanup HUD elements on level unload.
    }
}