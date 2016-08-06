using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class World : GameEntity
{
    private TrackingCamera2D _camera;
    private List<WorldEntity> _worldObjects;

    public TrackingCamera2D Camera
    {
        get { return _camera; }
    }

    protected override void OnInitialize()
    {
        _camera = FindObjectOfType<TrackingCamera2D>();
        Assert.IsNotNull(_camera, "Failed to find a GameObject with the TrackingCamera2D component.");

        _worldObjects = new List<WorldEntity>();
        foreach (var worldObject in FindObjectsOfType<WorldEntity>())
        {
            worldObject.Initialize(Game);
            _worldObjects.Add(worldObject);
        }
    }

    public void Update()
    {
        _worldObjects.ForEach(x => x.WorldUpdate());
    }

    public void FixedUpdate()
    {
        _worldObjects.ForEach(x => x.WorldFixedUpdate());
    }
}
