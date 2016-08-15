using UnityEngine;
using Framework;
using System.Collections.Generic;

public interface IHUDWorldElement
{
    void Initialize(WorldEntityManager worldEntityManager);
}

/// <summary>
/// Represents a HUD element that is bound to a GameEntity in the world.
/// </summary>
/// <typeparam name="TWorldEntity"></typeparam>
public abstract class HUDWorldElement<TWorldEntity> : MonoBehaviour, IHUDWorldElement where TWorldEntity : WorldEntity
{
    private WorldEntityListener<TWorldEntity> _entityListener;
    private List<TWorldEntity> _instances;

    void IHUDWorldElement.Initialize(WorldEntityManager worldEntityManager)
    {
        _instances = new List<TWorldEntity>();
        _entityListener = worldEntityManager.AddListener<TWorldEntity>();
        _entityListener.OnSpawn(Element_Spawned);
        _entityListener.OnRemove(Element_Destroyed);
        OnInitialize(worldEntityManager);
    }

    protected virtual void OnInitialize(WorldEntityManager worldEntityManager) { }

    private void Element_Spawned(TWorldEntity element)
    {
        element.Destroyed += Element_Destroyed;
        _instances.Add(element);
        OnElementSpawned(element);
    }

    private void Element_Destroyed(IWorldEntity gameEntity)
    {
        OnElementDestroyed(gameEntity as TWorldEntity);
        _instances.Remove(gameEntity as TWorldEntity);
    }

    protected virtual void OnElementSpawned(TWorldEntity element) { }
    protected virtual void OnElementDestroyed(TWorldEntity element) { }

    protected virtual void OnDestroy()
    {
        _instances.Clear();
    }
}
