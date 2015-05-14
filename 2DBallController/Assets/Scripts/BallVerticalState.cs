using UnityEngine;
using System.Collections;

public class BallVerticalState : MonoBehaviour
{
    public enum VerticalState { grounded, jumping, falling}
    public VerticalState currentState = VerticalState.falling;

    public PhysicsMaterial2D nonfriction;
    public PhysicsMaterial2D friction;

    public float maxAirtime = .2f;
    public float baseJumpForce = 2f;

    private Rigidbody2D _rb = null;
    private Vector2 _velocity = Vector2.zero;
    private Collider2D _circleCollider = null;
    private float _jumpForce = 5f;
    private float _airtime = 0f;
    private bool _spaceBar = false;

    #region MonobehaviorMessages
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update ()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _spaceBar = true;
        }
        if(Input.GetButtonUp("Jump"))
        {
            _spaceBar = false;
        }
    }

    void FixedUpdate ()
    {
        _velocity = _rb.velocity;
        switch(currentState)
        {
            case VerticalState.grounded:
                if (_spaceBar)
                {
                    transitionToJumping();
                }
                break;
            case VerticalState.jumping:
                if (_spaceBar && _jumpForce > 0)
                {
                    ApplyJumpForce();
                }
                else
                    transitionToFalling();
                break;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {

        if (other != null)
        {
            float dotProduct = Vector2.Dot(other.contacts[0].normal, Vector2.up);
            if (!isGrounded() && !isJumping() && dotProduct > .8)
            {
                //Debug.Log("collision");
                transitionToGrounded();
            }
            //The cause of the collision bug
            //else if (isGrounded() && dotProduct < .8)
            //{
            //    transitionToFalling();
            //}
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (isGrounded())
        {
            transitionToFalling();
        }
    }
    #endregion

    #region OtherFunctions
    public void SwitchPhysMat2D(Collider2D _collider, PhysicsMaterial2D _material)
    {
        //Debug.Log("Switched Physical Material");
        _collider.sharedMaterial = _material;
        _collider.enabled = false;
        _collider.enabled = true;
    }

    void ApplyJumpForce ()
    {
        //Debug.Log("Applying Jump Force");
        _rb.velocity += new Vector2(0f,_jumpForce);
        _jumpForce *= _airtime;
        _airtime -= .1f * Time.fixedDeltaTime;
    }
    #endregion

    #region StateTransitions
    void transitionToGrounded()
    {
        currentState = VerticalState.grounded;
    }

    void transitionToJumping()
    {
        //Debug.Log("Engaging Jump");
        _velocity.y = 0f;
        _rb.velocity = _velocity;
        _jumpForce = baseJumpForce;
        _airtime = maxAirtime;
        SwitchPhysMat2D(_circleCollider, nonfriction);
        currentState = VerticalState.jumping;
    }

    void transitionToFalling()
    {
        if (_circleCollider.sharedMaterial == nonfriction)
        {
            SwitchPhysMat2D(_circleCollider, friction);
        }

        currentState = VerticalState.falling;
    }
    #endregion

    #region StateChecks
    bool isGrounded()
    {
        return currentState == VerticalState.grounded;
    }

    bool isJumping()
    {
        return currentState == VerticalState.jumping;
    }

    bool isFalling()
    {
        return currentState == VerticalState.falling;
    }
    #endregion

}

