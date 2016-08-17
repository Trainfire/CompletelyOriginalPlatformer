using System;
using Framework;
using Framework.UI;

public class HUDInteractableAreas : HUDWorldElement<InteractableArea>
{
    public UIInteractPoint Prototype;

    private UIInteractPoint _instance;

    protected override void OnInitialize(WorldEntityManager worldEntityManager)
    {
        base.OnInitialize(worldEntityManager);

        _instance = UIUtility.Add<UIInteractPoint>(transform, Prototype.gameObject);
        _instance.gameObject.SetActive(false);
    }

    protected override void OnElementSpawned(InteractableArea element)
    {
        base.OnElementSpawned(element);

        element.Entered += Element_TriggerEnter;
        element.Left += Element_TriggerLeave;
    }

    protected override void OnElementDestroyed(InteractableArea element)
    {
        base.OnElementDestroyed(element);

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
}