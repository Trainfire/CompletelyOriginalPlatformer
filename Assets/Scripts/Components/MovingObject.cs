using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Attach this to any moving object that the player can stand on.
/// </summary>
class MovingObject : MonoBehaviour
{
    [SerializeField] private SliderJoint2D _sliderJoint;

    private Player _player;
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        Assert.IsNotNull(_collider);
    }

    void Disconnect(Player player)
    {
        _sliderJoint.connectedBody = null;
    }

    void ConnectTo(Rigidbody2D character)
    {
        _sliderJoint.connectedBody = character;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (collision.contacts[0].normal != Vector2.down)
                return;

            _player = collision.collider.GetComponent<Player>();
            _player.Controller.Jumped += Controller_Jumped;

            ConnectTo(collision.collider.GetComponent<Rigidbody2D>());
        }
    }

    void FixedUpdate()
    {
        if (_sliderJoint.connectedBody != null && _player != null)
        {
            var breakingDistance = _collider.bounds.extents.x;

            if (Mathf.Abs(transform.position.x - _sliderJoint.connectedBody.transform.position.x) >= breakingDistance)
                Disconnect(_player);
        }
            
    }

    void Controller_Jumped(PlayerController controller)
    {
        controller.Jumped -= Controller_Jumped;
        Disconnect(_player);
    }
}
