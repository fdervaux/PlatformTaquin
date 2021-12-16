using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{

    //private CharacterController _characterController = null;
    private PlayerInput _PlayerInput = null;

    private InputAction _moveAction = null;
    private InputAction _JumpAcion = null;


    [Range(1, 10)] public float _speed;

    private const float EPSILON = 0.1f;

    private Vector3 _velocity = Vector3.zero;
    [Range(1, 8)] public float _jumpForce = 3;

    [Range(0, 1)] public float _airControl = 0.05f;

    public float _gravityFactorJumpUp = 1;

    public float _gravityFactorJumpDown = 1.4f;

    public float _gravityFactorCancelJump = 1.6f;

    private bool _isGrounded = false;

    public bool _is2dSideScroller = false;

    private Vector3 _groundVelocity = Vector3.zero;

    public float _groundCheckDistance = 0.4f;

    public float _groundCheckOffset = 1.0f;

    public MoveToTarget _moveToTarget = null;
    public MovePlatform _movePlatform = null;

    private Rigidbody _rigidBody;

    private bool startJump = false;



    public void OnJump()
    {
        //add jump force to the player velocity
        if (_isGrounded)
        {
            _velocity += _jumpForce * Vector3.up + _groundVelocity;
            startJump = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        _rigidBody = GetComponent<Rigidbody>();
        _PlayerInput = GetComponent<PlayerInput>();

        _moveAction = _PlayerInput.actions.FindAction("Move", true);
        _JumpAcion = _PlayerInput.actions.FindAction("Jump", true);
    }


    private void FixedUpdate()
    {
        //check the floor under the player
        Vector3 groundCheckCorrection = Vector3.zero;
        if (_moveToTarget != null)
        {
            groundCheckCorrection = _moveToTarget.velocity() * Time.fixedDeltaTime;
            _rigidBody.MovePosition( _rigidBody.position + groundCheckCorrection);
        }

        if (_movePlatform != null)
        {
            groundCheckCorrection = _movePlatform.velocity() * Time.fixedDeltaTime;
            _rigidBody.MovePosition( _rigidBody.position + groundCheckCorrection);
        }
        

        Vector3 groundCorrection = Vector3.zero;
        _isGrounded = false;
        _groundVelocity = Vector3.zero;
        _moveToTarget = null;
        _movePlatform = null;
        

        if (!startJump && _velocity.y < EPSILON )
        {
            RaycastHit hit;
            if (Physics.Raycast(_rigidBody.position + Vector3.up * _groundCheckOffset, Vector3.down, out hit, _groundCheckDistance + _groundCheckOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore ))
            {
                _isGrounded = true;
                groundCorrection = Vector3.down * (hit.distance - _groundCheckOffset);
                MovePlatform platform = hit.transform.GetComponent<MovePlatform>();
                if (platform)
                {
                    _groundVelocity = platform.velocity();
                    _movePlatform = platform;
                }

                MoveToTarget moveToTarget = hit.transform.GetComponent<MoveToTarget>();
                if (moveToTarget)
                {
                    _moveToTarget = moveToTarget;
                    _groundVelocity = moveToTarget.velocity();
                }
            }
        }
    
        //Retrieve player input
        Vector2 inputMove = _moveAction.ReadValue<Vector2>();
        float inputJump = _JumpAcion.ReadValue<float>();

        //change gravity factor for better jump
        float gravityFactor = 1.0f;

        if (_velocity.y < 0)
        {
            gravityFactor = _gravityFactorJumpDown;
        }
        else if (inputJump > 0.5f)
        {
            gravityFactor = _gravityFactorJumpUp;
        }
        else
        {
            gravityFactor = _gravityFactorCancelJump;
        }


        //apply gravity to the character
        if (!_isGrounded)
        {
            _velocity += Vector3.down * 9.81f * gravityFactor * Time.fixedDeltaTime;
        }
        else if (_velocity.y < EPSILON)
        {
            _velocity.y = 0;
        }

        // apply horizontal move to the player accorrding to the inputs
        if (_isGrounded)
        {
            _velocity.x = inputMove.x * _speed;
            if (!_is2dSideScroller)
                _velocity.z = inputMove.y * _speed;
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, inputMove.x * _speed, _airControl);
            if (!_is2dSideScroller)
                _velocity.z = Mathf.MoveTowards(_velocity.z, inputMove.y * _speed, _airControl);
        }

        if (!_is2dSideScroller)
        {
            if (inputMove.magnitude > EPSILON)
            {
                float angle = Vector2.SignedAngle(new Vector2(1, 0), inputMove);
                transform.rotation = Quaternion.Euler(0, -angle, 0);
            }
        }
        else
        {
            if (inputMove.x > EPSILON)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (inputMove.x < -EPSILON)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        startJump = false;
        
        //mover character for better physics
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.MovePosition(_rigidBody.position + _velocity * Time.fixedDeltaTime + groundCorrection);
        
    }
}
