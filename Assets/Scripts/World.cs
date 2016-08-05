using UnityEngine;
using UnityEngine.Assertions;
using Framework;

public class World : MonoBehaviour
{
    private TrackingCamera2D _camera;

    public TrackingCamera2D Camera
    {
        get { return _camera; }
    }

    private void Awake()
    {
        _camera = FindObjectOfType<TrackingCamera2D>();
        Assert.IsNotNull(_camera, "Failed to find a GameObject with the TrackingCamera2D component.");
    }
}
