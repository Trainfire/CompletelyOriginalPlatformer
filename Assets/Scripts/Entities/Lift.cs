using UnityEngine;
using Framework;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Lift : MonoBehaviour
{
    [SerializeField] private Trigger _trigger;
    [SerializeField] private List<Transform> _points;
    [SerializeField] private float _moveSpeed;

    private Rigidbody2D _rigidBody;
    private float _distanceTravelled;
    private float _targetDistance;
    private Vector3 _target;
    private Vector3 _direction;
    private int _index;

    private Orientation _orientation;
    enum Orientation
    {
        Forwards,
        Backwards,
    }

    private State _state;
    enum State
    {
        Idle,
        Moving
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _trigger.Triggered += Move;

        if (_points == null)
            _points = new List<Transform>();

        if (_points.Count == 0)
        {
            Debug.LogWarning("Lift points contains no items.");
        }
        else
        {
            transform.position = _points[0].position;
        }            
    }

    private void Move(Trigger trigger)
    {
        if (_points.Count == 0)
        {
            Debug.LogWarning("Lift cannot move as no points are available.");
            return;
        }

        if (transform.position == _points[0].position)
        {
            _orientation = Orientation.Forwards;
            MoveNext();
        }
        else if (transform.position == _points[_points.Count - 1].position)
        {
            _orientation = Orientation.Backwards;
            MoveNext();
        }
        else
        {
            _orientation = _orientation == Orientation.Forwards ? Orientation.Backwards : Orientation.Forwards;
            MoveNext();
        }
    }

    private void FixedUpdate()
    {
        if (_state == State.Idle)
            return;

        float moveSpeed = _moveSpeed * Time.deltaTime;
        _rigidBody.MovePosition(transform.position += _direction * moveSpeed);
        _distanceTravelled += moveSpeed;

        if (_distanceTravelled >= _targetDistance)
        {
            transform.position = _target;
            MoveNext();
        }
    }

    private void MoveNext()
    {
        if (_orientation == Orientation.Forwards)
        {
            if (!_points.InRange(_index + 1))
            {
                _state = State.Idle;
                return;
            }
            _index++;
        }
        else
        {
            if (!_points.InRange(_index - 1))
            {
                _state = State.Idle;
                return;
            }
            _index--;
        }

        _target = _points[_index].position;
        _direction = (_target - transform.position).normalized;
        _targetDistance = Vector3.Distance(transform.position, _target);
        _distanceTravelled = 0f;

        _state = State.Moving;
    }
}
