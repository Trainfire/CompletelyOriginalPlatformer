using UnityEngine;
using Framework;
using System;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour _prototype;

    [SerializeField]
    private float _delay;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(_delay);
        var instance = Instantiate(_prototype);
        instance.transform.position = transform.position;
        yield return StartCoroutine(Spawn());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}