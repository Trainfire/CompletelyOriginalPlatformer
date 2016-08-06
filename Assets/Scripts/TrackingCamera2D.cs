using UnityEngine;
using UnityEngine.Assertions;
using Framework;
using Framework.Components;
using System.Collections.Generic;

public class TrackingCamera2D : GameEntity
{
    [SerializeField] private GameObject _initialTarget;
    [Range(0f, 1f)] [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector2 _offset;

    private GameObject _trackingTarget;
    private Vector2 _worldToScreen;
    private Vector2 _lerp;

    private Camera _camera;
    private List<ScreenEffect> _screenEffects;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        Assert.IsNotNull(_camera);

        _screenEffects = new List<ScreenEffect>();

        if (_initialTarget != null)
            _trackingTarget = _initialTarget;
    }

    private void Update()
    {
        if (_trackingTarget == null)
            return;

        _moveSpeed = Mathf.Clamp(_moveSpeed, 0f, 1f);

        // Get target in screen space and centralise.
        _worldToScreen = _camera.WorldToScreenPoint(_trackingTarget.transform.position).ToVec2();
        _worldToScreen += new Vector2(-Screen.width / 2f, -Screen.height / 2f) + _offset;

        _lerp = Vector2.Lerp(_camera.transform.position, _worldToScreen, Time.deltaTime * _moveSpeed);

        // Set position ignoring Z.
        _camera.transform.position = new Vector3(_lerp.x, _lerp.y, _camera.transform.position.z);

        _screenEffects.ForEach(x => x.ProcessEffect());
    }

    public T AddScreenEffect<T>() where T : ScreenEffect
    {
        var effect = gameObject.AddComponent<T>();
        effect.Finished += ScreenEffect_Finished;
        _screenEffects.Add(effect);
        return effect;
    }

    private void ScreenEffect_Finished(ScreenEffect screenEffect)
    {
        Assert.IsTrue(_screenEffects.Contains(screenEffect));

        if (_screenEffects.Contains(screenEffect))
        {
            _screenEffects.Remove(screenEffect);
            Destroy(screenEffect);
        }
    }

    public void SetTarget(GameObject target)
    {
        _trackingTarget = target;
    }
}
