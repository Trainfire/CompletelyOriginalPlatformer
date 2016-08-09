using UnityEngine;
using Framework;
using System.Collections.Generic;
using System;

public abstract class HUDWorldElement : MonoBehaviour
{
    public abstract void Cleanup();
}

/// <summary>
/// Represents a HUD element that is bound to a GameEntity in the world.
/// </summary>
/// <typeparam name="TGameEntity"></typeparam>
public abstract class HUDWorldElement<TGameEntity> : HUDWorldElement where TGameEntity : GameEntity
{
    private GameEntityListener<TGameEntity> _entityListener;
    private List<TGameEntity> _instances;

    protected virtual void Awake()
    {
        _instances = new List<TGameEntity>();
        _entityListener = new GameEntityListener<TGameEntity>();
        _entityListener.Spawned += Element_Spawned;
    }

    private void Element_Spawned(TGameEntity element)
    {
        element.Destroyed += Element_Destroyed;
        _instances.Add(element);
        OnElementSpawned(element);
    }

    private void Element_Destroyed(GameEntity gameEntity)
    {
        OnElementDestroyed(gameEntity as TGameEntity);
        _instances.Remove(gameEntity as TGameEntity);
    }

    protected virtual void OnElementSpawned(TGameEntity element) { }
    protected virtual void OnElementDestroyed(TGameEntity element) { }

    public override void Cleanup()
    {
        // TODO.
    }
}