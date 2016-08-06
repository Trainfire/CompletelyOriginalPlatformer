using UnityEngine;
using Framework;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour, IInputHandler
{
    // TODO: Expose events here for the animator to hook into.
    public event Action<LandEventArgs> Landed;

    public class LandEventArgs : EventArgs
    {
        public float Velocity { get; private set; }

        public LandEventArgs(float velocity)
        {
            Velocity = velocity;
        }
    }

    [SerializeField] private float _hAccelRate;
    [SerializeField] private float _hDeaccelRate;
    [SerializeField] private float _hMaxSpeed;
    [SerializeField] private float _vForce;
    [SerializeField] private LayerMask _worldMask;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private Vector3 _cachedVelocity;
    private float _hAccel;
    private float _hVelocity;
    private Vector2 _facingDirection;

    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _blocked;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public bool Enabled
    {
        set { _rigidBody.isKinematic = !value; }
    }

    private Vector2 Ground
    {
        get { return _collider.bounds.extents.x / 2f * Vector2.down; }
    }

    public void HandleInput(InputActionEvent action)
    {
        if (action.Type == InputActionType.Down)
        {
            if (action.Action == InputAction.Jump)
                Jump();
        }

        if (action.Type == InputActionType.Held && !_blocked)
        {
            if (action.Action == InputAction.Left)
            {
                _facingDirection = Vector2.left;
            }
            else if (action.Action == InputAction.Right)
            {
                _facingDirection = Vector2.right;
            }
        }

        if (action.Type == InputActionType.Up)
        {
            if (action.Action == InputAction.Left || action.Action == InputAction.Right)
                _facingDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(transform.position.ToVec2() + (_collider.bounds.extents.y / 2f * Vector2.down), _collider.bounds.extents.y, _worldMask);
        _blocked = Physics2D.OverlapCircle(transform.position.ToVec2() + (_collider.bounds.extents.x / 2f * _facingDirection), _collider.bounds.extents.x, _worldMask);

        if (_wasGrounded == false && _isGrounded == true && Landed != null)
            Landed(new LandEventArgs(_cachedVelocity.y));

        // Horizontal Acceleration
        if (Mathf.Abs(_facingDirection.x) > 0f && !_blocked)
        {
            // Accel.
            if (_hAccel < _hMaxSpeed)
                _hAccel += Time.deltaTime * _hAccelRate;
        }
        else
        {
            // Deaccel.
            if (_hAccel > 0f)
                _hAccel -= Time.deltaTime * _hDeaccelRate;
        }

        // Reset horizontal velocity when blocked in the direction that we're trying to move in.
        if (_blocked)
        {
            _hVelocity = 0f;
        }
        else
        {
            // Increase horizontal velocity.
            _hVelocity += _facingDirection.x * _hAccel;
            _hVelocity = Mathf.Clamp(_hVelocity, -_hAccel, _hAccel);
        }

        _rigidBody.velocity = new Vector2(_hVelocity, _rigidBody.velocity.y);

        _wasGrounded = _isGrounded;
        _cachedVelocity = _rigidBody.velocity;
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _isGrounded = false;
            _rigidBody.AddForce(Vector2.up * _vForce);
        }
    }
}
