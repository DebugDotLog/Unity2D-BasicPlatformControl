using UnityEngine;
using System.Collections;

public class BallVerticalState : MonoBehaviour
{
        // Setup and Create the state machine with 3 states. currentState will provide a live state monitor in Unity's Inspector.
    public enum VerticalState { grounded, jumping, falling }
    public VerticalState currentState = VerticalState.falling;

        // PhysicsMaterial2Ds used to alter collider's friction while jumping.
    public PhysicsMaterial2D nonfriction;
    public PhysicsMaterial2D friction;

        // Tweak these to alter the feel/height/response of the jump.
    public float maxAirtime = .8f;
    public float baseJumpForce = 2f;

    private Rigidbody2D _rb = null;
    private Vector2 _velocity = Vector2.zero;
    private Collider2D _circleCollider = null;
    private float _jumpForce = 0f;
    private float _airtime = 0f;
    private bool _spaceBar = false;

    #region MonobehaviorMessages

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _spaceBar = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            _spaceBar = false;
        }
    }

    void FixedUpdate()
    {
        _velocity = _rb.velocity;

            // Switch statement controls flow depending on the current state.
        switch (currentState)
        {
            case VerticalState.grounded:
                if (_spaceBar)
                {
                    TransitionToJumping();
                }
                break;
            case VerticalState.jumping:
                if (_spaceBar && _jumpForce > .0001f)
                {
                    ApplyJumpForce();
                }
                else
                    TransitionToFalling();
                break;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {

        if (other != null)
        {
            float dotProduct = Vector2.Dot(other.contacts[0].normal, Vector2.up);   // The dotProduct of a flat horizontal surface will be ~1
            if (IsFalling() && dotProduct > .8)                                     // We want to be falling before we check to see if we're grounded.
            {
                //Debug.Log("collIsion");
                TransitionToGrounded();
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (IsGrounded())                                               // Make sure we go into the falling state when we fall off without jumping first.
        {
            TransitionToFalling();
        }
    }
    #endregion

    #region OtherFunctions

    /// <summary>
    /// Switches _collider's physics material to _material
    /// </summary>
    public void SwitchPhysMat2D(Collider2D _collider, PhysicsMaterial2D _material)
    {
        //Debug.Log("Switched Physical Material");
        _collider.sharedMaterial = _material;
        _collider.enabled = false;
        _collider.enabled = true;
    }

    /// <summary>
    /// Makes local _rb jump by _jumpForce. Multiplies _jumpForce by _airtime. Decrements Airtime.
    /// </summary>
    void ApplyJumpForce()
    {
        //Debug.Log("Applying Jump Force");
        _rb.velocity += new Vector2(0f, _jumpForce);
        _jumpForce *= _airtime;
        _airtime -= .1f * Time.fixedDeltaTime;
    }
    #endregion

    #region StateTransitions

    /// <summary>
    /// Changes currentState to grounded.
    /// </summary>
    void TransitionToGrounded()
    {
        currentState = VerticalState.grounded;
    }

    /// <summary>
    /// Makes preparations for the beginning of a jump, then sets currentState to jumping.
    /// </summary>
    void TransitionToJumping()
    {
        //Debug.Log("Engaging Jump");
        _velocity.y = 0f;
        _rb.velocity = _velocity;
        _jumpForce = baseJumpForce;
        _airtime = maxAirtime;
        SwitchPhysMat2D(_circleCollider, nonfriction);
        currentState = VerticalState.jumping;
    }

    /// <summary>
    /// Makes sure the collider's material has friction, then sets currentState to falling.
    /// </summary>
    void TransitionToFalling()
    {
        if (_circleCollider.sharedMaterial == nonfriction)
        {
            SwitchPhysMat2D(_circleCollider, friction);
        }

        currentState = VerticalState.falling;
    }
    #endregion

    #region StateChecks

    /// <summary>
    /// Returns true of currentState is grounded.
    /// </summary>
    bool IsGrounded()
    {
        return currentState == VerticalState.grounded;
    }

    /// <summary>
    /// Returns true if currentState is jumping.
    /// </summary>
    bool IsJumping()
    {
        return currentState == VerticalState.jumping;
    }

    /// <summary>
    /// Returns true if currentState is falling.
    /// </summary>
    bool IsFalling()
    {
        return currentState == VerticalState.falling;
    }
    #endregion

}

