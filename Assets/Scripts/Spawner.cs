using UnityEngine;
using Framework;
using System;
using System.Collections;

public class Spawner : WorldEntity
{
    [SerializeField]
    private WorldEntity _prototype;

    [SerializeField]
    private float _delay;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(_delay);
        var instance = World.Entities.Spawn(_prototype);
        instance.transform.position = transform.position;
        yield return StartCoroutine(Spawn());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }
}