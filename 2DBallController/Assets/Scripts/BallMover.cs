using UnityEngine;
using System.Collections;

public class BallMover : MonoBehaviour
{

    public float deltaChangeDirections = .035f;
    public float deltaAcceleration = .027f;
    public float maximumVelocity = 3f;

    private float _movement = 0f;
    private Rigidbody2D _rb = null;
    private Vector2 _velocity = Vector2.zero;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _velocity = _rb.velocity;
    }

    void Update()
    {
        _movement = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        _velocity = _rb.velocity;
        if (_movement > 0)
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_velocity.x, maximumVelocity, _velocity.x < 0 ? deltaChangeDirections : deltaAcceleration), _velocity.y);
        }
        else if (_movement < 0)
        {
            _rb.velocity = new Vector2(Mathf.Lerp(_velocity.x, -maximumVelocity, _velocity.x > 0 ? deltaChangeDirections : deltaAcceleration), _velocity.y);
        }
        else
        {
            if (Mathf.Abs(_velocity.x) < 1)
            {
                _rb.velocity = new Vector2(Mathf.MoveTowards(_velocity.x, 0, .05f), _velocity.y); 
            }
        }
        //Debug.Log(_velocity.x);
    }
}
