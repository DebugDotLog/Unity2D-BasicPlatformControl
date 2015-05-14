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

        // Here we check to see which way the player wants to go. We then adjust the x velocity of the player character using Lerp, from
        // its current velocity to its maximum velocity. The changes in t allow the player character to reverse direction at a different
        // rate than it accelerates in one direction, from 0. In the default state, it changes direction rapidly, then accelerates moderately.

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
            if (Mathf.Abs(_velocity.x) < 1)         // If the player isn't accelerating and the player character's speed is between -1 and 1,
            {                                       // we will adjust the velocity - quickly stepping down to 0 in increments of .05/fixedupdate.
                _rb.velocity = new Vector2(Mathf.MoveTowards(_velocity.x, 0, .05f), _velocity.y); 
            }
        }
        //Debug.Log(_velocity.x);
    }
}
