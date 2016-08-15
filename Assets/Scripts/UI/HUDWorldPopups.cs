﻿using System;
using Framework;
using Framework.UI;

public class HUDWorldPopups : HUDWorldElement<WorldPopup>
{
    public UIWorldPopup Prototype;

    private UIWorldPopup _popupInstance;

    protected override void OnInitialize(WorldEntityManager worldEntityManager)
    {
        base.OnInitialize(worldEntityManager);
        _popupInstance = UIUtility.Add<UIWorldPopup>(transform, Prototype.gameObject);
        _popupInstance.gameObject.SetActive(false);
    }

    protected override void OnElementSpawned(WorldPopup element)
    {
        base.OnElementSpawned(element);
        element.TriggerEnter += Element_TriggerEnter;
        element.TriggerLeave += Element_TriggerLeave;
    }

    protected override void OnElementDestroyed(WorldPopup element)
    {
        base.OnElementDestroyed(element);
        element.TriggerEnter -= Element_TriggerEnter;
        element.TriggerLeave -= Element_TriggerLeave;
    }

    private void Element_TriggerEnter(WorldPopup worldPopup)
    {
        _popupInstance.Content = worldPopup.Content;
        _popupInstance.SetVisibility(true);
    }

    private void Element_TriggerLeave(WorldPopup worldPopup)
    {
        _popupInstance.Content = string.Empty;
        _popupInstance.SetVisibility(false);
    }
}