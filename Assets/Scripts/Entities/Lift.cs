using UnityEngine;
using Framework;

public class Lift : MonoBehaviour
{
    [SerializeField]
    private Trigger _trigger;

    [SerializeField]
    private Transform _first;

    [SerializeField]
    private Transform _last;

    [SerializeField]
    private float _moveSpeed;

    private float _distanceTravelled;
    private float _targetDistance;
    private Vector3 _target;
    private Vector3 _direction;

    private State _state;
    enum State
    {
        Idle,
        Moving
    }

    void Awake()
    {
        _trigger.Triggered += Move;
    }

    private void Move(Trigger trigger)
    {
        if (_state == State.Moving)
        {
            _direction = _direction == Vector3.up ? Vector3.down : Vector3.up;
            _target = _direction == Vector3.up ? _last.position : _first.position;
        }
        else
        {
            if (transform.position.y <= _first.position.y)
            {
                _target = _last.position;
                _direction = Vector3.up;
            }
            else
            {
                _target = _first.position;
                _direction = Vector3.down;
            }
        }

        _distanceTravelled = 0f;
        _targetDistance = Vector3.Distance(transform.position, _target);
        _state = State.Moving;
    }

    private void Update()
    {
        if (_state == State.Idle)
            return;

        float moveSpeed = _moveSpeed * Time.deltaTime;
        transform.position += _direction * moveSpeed;
        _distanceTravelled += moveSpeed;

        if (_distanceTravelled >= _targetDistance)
            _state = State.Idle;
    }
}
