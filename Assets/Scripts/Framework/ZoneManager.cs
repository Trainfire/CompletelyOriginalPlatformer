using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Framework;

public class ZoneManager<T> : MonoBehaviour
{
    private IZoneHandler<T> _handler;
    private List<string> _activeScenes;

    public ZoneListener<T> Listener { get; private set; }
    public T Zone { get; private set; }

    protected virtual void Awake()
    {
        Listener = new ZoneListener<T>();
        _activeScenes = new List<string>();
        _handler = Listener;
    }

    public void SetZone(T zone, params string[] sceneNames)
    {
        StartCoroutine(SetZoneAsync(zone, sceneNames));
    }

    IEnumerator SetZoneAsync(T zone, params string[] sceneNames)
    {
        // TODO: Load loading screen here...

        _handler.OnZoneChanging();

        _activeScenes.ForEach(x => SceneManager.UnloadScene(x));
        _activeScenes.Clear();

        foreach (var scene in sceneNames)
        {
            _activeScenes.Add(scene);

            var task = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            while (!task.isDone)
            {
                // TODO: Progress here.
                yield return null;
            }
        }

        Zone = zone;

        _handler.OnZoneChanged(zone);
    }
}

public interface IZoneHandler<T>
{
    void OnZoneChanging();
    void OnZoneLoadProgress(float progress);
    void OnZoneChanged(T zone);
}

public class ZoneListener<T> : IZoneHandler<T>
{
    public event Action ZoneChanging;
    public event Action<float> ZoneLoadProgress;
    public event Action<T> ZoneChanged;

    void IZoneHandler<T>.OnZoneChanged(T zone)
    {
        ZoneChanged.InvokeSafe(zone);
    }

    void IZoneHandler<T>.OnZoneChanging()
    {
        ZoneChanging.InvokeSafe();
    }

    void IZoneHandler<T>.OnZoneLoadProgress(float progress)
    {
        ZoneLoadProgress.InvokeSafe(progress);
    }
}