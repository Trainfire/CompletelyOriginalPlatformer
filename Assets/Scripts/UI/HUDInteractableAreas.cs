using UnityEngine;
using Framework;
using Framework.UI;
using System.Collections.Generic;

public class HUDInteractableAreas : HUDWorldElement<InteractableArea>
{
    [SerializeField] private UIInteractPoint _prototype;
    [SerializeField] private Vector2 _offset;

    private InteractableArea _element;
    private UIInteractPoint _instance;
    private RectTransform _instanceRect;

    protected override void OnInitialize(List<InteractableArea> elements)
    {
        base.OnInitialize(elements);

        _instance = UIUtility.Add<UIInteractPoint>(transform, _prototype.gameObject);
        _instance.gameObject.SetActive(false);

        // Cache this for later.
        _instanceRect = _instance.transform as RectTransform;

        foreach (var element in Elements)
        {
            element.Entered += Element_TriggerEnter;
            element.Left += Element_TriggerLeave;
        }
    }

    private void Element_TriggerEnter(InteractableArea interactableArea)
    {
        _instance.Show(interactableArea.Message);
        _element = interactableArea;
    }

    private void Element_TriggerLeave(InteractableArea interactableArea)
    {
        _instance.SetVisibility(false);
        _element = null;
    }

    private void Update()
    {
        if (_instance != null && _element != null)
        {
            _instanceRect.anchoredPosition = Canvas.WorldToCanvasPoint(_element.transform.position) + _offset;
        }
    }
}