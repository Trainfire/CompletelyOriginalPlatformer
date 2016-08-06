using UnityEngine;
using Framework;
using System;

public class WorldEntity : GameEntity
{
    public void WorldUpdate()
    {
        OnWorldUpdate();
    }

    public void WorldFixedUpdate()
    {
        OnWorldFixedUpdate();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected virtual void OnWorldUpdate() { }
    protected virtual void OnWorldFixedUpdate() { }
}
