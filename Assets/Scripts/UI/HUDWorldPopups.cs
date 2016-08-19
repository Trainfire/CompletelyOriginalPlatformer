using System;
using System.Collections.Generic;
using Framework;
using Framework.UI;

public class HUDWorldPopups : HUDWorldElement<WorldPopup>
{
    public UIWorldPopup Prototype;

    private UIWorldPopup _popupInstance;

    protected override void OnInitialize(List<WorldPopup> elements)
    {
        base.OnInitialize(elements);

        _popupInstance = UIUtility.Add<UIWorldPopup>(transform, Prototype.gameObject);
        _popupInstance.gameObject.SetActive(false);

        foreach (var element in elements)
        {
            element.TriggerEnter += Element_TriggerEnter;
            element.TriggerLeave += Element_TriggerLeave;
        }
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

    private void OnDestroy()
    {
        foreach (var element in Elements)
        {
            element.TriggerEnter -= Element_TriggerEnter;
            element.TriggerLeave -= Element_TriggerLeave;
        }
    }
}