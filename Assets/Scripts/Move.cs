using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{

    private CharacterController _characterController = null;
    private PlayerInput _PlayerInput = null;

    private InputAction _moveAction = null;
    private InputAction _JumpAcion = null;


    [Range(1, 10)] public float _speed;

    private const float EPSILON = 0.1f;

    private Vector3 _velocity = Vector3.zero;
    [Range(1, 8)] public float _jumpForce = 3;

    [Range(0, 1)] public float _airControl = 0.05f;

    public float gravityFactorJumpUp = 1;

    public float gravityFactorJumpDown = 1.4f;

    public float gravityFactorCancelJump = 1.6f;

    private bool _isGrounded = false;

    public void OnJump()
    {
        //add jump force to the player velocity
        if (_isGrounded)
        {
            _velocity.y += _jumpForce;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _PlayerInput = GetComponent<PlayerInput>();

        _moveAction = _PlayerInput.actions.FindAction("Move");
        _JumpAcion = _PlayerInput.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        //check the floor under the player
        Vector3 platformVelocity = Vector3.zero;
        Vector3 groundCorrection = Vector3.zero;

        _isGrounded = false;

        if (_velocity.y < EPSILON)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 0.5f))
            {
                _isGrounded = true;
                groundCorrection = Vector3.down * (hit.distance - 0.2f);
                MovePlatform _platform = hit.transform.GetComponent<MovePlatform>();
                if (_platform)
                {
                    platformVelocity = _platform.velocity();
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
            gravityFactor = gravityFactorJumpDown;
        }
        else if (inputJump > 0.5f)
        {
            gravityFactor = gravityFactorJumpUp;
        }
        else
        {
            gravityFactor = gravityFactorCancelJump;
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
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, inputMove.x * _speed, _airControl);
        }


        //change direction of the player according to the input
        if (inputMove.x > EPSILON)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (inputMove.x < -EPSILON)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }


        //mover character for better physics
        _characterController.Move((_velocity + platformVelocity) * Time.fixedDeltaTime + groundCorrection);
    }
}
