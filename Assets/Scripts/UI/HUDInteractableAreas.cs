using UnityEngine;
using Framework;
using Framework.UI;

public class HUDInteractableAreas : HUDWorldElement<InteractableArea>
{
    [SerializeField] private UIInteractPoint _prototype;
    [SerializeField] private Vector2 _offset;

    private InteractableArea _element;
    private UIInteractPoint _instance;
    private RectTransform _instanceRect;

    protected override void OnInitialize(WorldEntityManager worldEntityManager)
    {
        base.OnInitialize(worldEntityManager);

        _instance = UIUtility.Add<UIInteractPoint>(transform, _prototype.gameObject);
        _instance.gameObject.SetActive(false);

        // Cache this for later.
        _instanceRect = _instance.transform as RectTransform;
    }

    protected override void OnElementSpawned(InteractableArea element)
    {
        base.OnElementSpawned(element);

        _element = element;

        element.Entered += Element_TriggerEnter;
        element.Left += Element_TriggerLeave;
    }

    protected override void OnElementDestroyed(InteractableArea element)
    {
        base.OnElementDestroyed(element);

        _element = element;

        element.Entered -= Element_TriggerEnter;
        element.Left -= Element_TriggerLeave;
    }

    private void Element_TriggerEnter(InteractableArea interactableArea)
    {
        _instance.Show(interactableArea.Message);
    }

    private void Element_TriggerLeave(InteractableArea interactableArea)
    {
        _instance.SetVisibility(false);
    }

    private void Update()
    {
        if (_instance != null && _element != null)
        {
            _instanceRect.anchoredPosition = Canvas.WorldToCanvasPoint(_element.transform.position) + _offset;
        }
    }
}