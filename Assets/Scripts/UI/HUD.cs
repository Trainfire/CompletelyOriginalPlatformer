using UnityEngine;
using System.Collections.Generic;
using Framework;
using Framework.UI;

public class HUD : GameControllerDependant
{
    [SerializeField] private HUDWorldPopups _hudPopups;

    private List<GameObject> _hudElements;

    protected override void OnInitialize()
    {
        _hudElements = new List<GameObject>();
        _hudElements.Add(_hudPopups.gameObject);
    }

    public void OnDestroy()
    {
        _hudElements.Clear();
    }
}