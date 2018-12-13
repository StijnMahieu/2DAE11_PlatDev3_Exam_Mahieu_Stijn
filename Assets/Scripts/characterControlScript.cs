using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class characterControlScript : MonoBehaviour
{
    //animations
    private Animator _animator;
    //charactercontroller
    private CharacterController _characterController;

    //velocity
    private Vector3 _velocity = Vector3.zero;

    //movement
    private Vector3 _movement = Vector3.zero;

    //jump
    private bool _jump;
    [SerializeField]
    private float _jumpHeight = 1;

    //LocomotionParameters
    [SerializeField]
    private float _mass = 75;
    [SerializeField]
    private float _acceleration = 3;
    [SerializeField]
    private float _dragOnGround;
    [SerializeField]
    private float _maxRunningSpeed = (30.0f * 1000) / (60 * 60);

    //Dependencies
    [SerializeField]
    private Transform _absoluteForward;

    //animation
    private int _horizontalVelocityParameter = Animator.StringToHash("HorizontalVelocity");
    private int _verticalVelocityParameter = Animator.StringToHash("VerticalVelocity");

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if(Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
    }

    void FixedUpdate()
    {
        ApplyGravity();
        ApplyGround();
        ApplyMovement();
        ApplyGroundDrag();
        LimitMaximumRunningSpeed();
        ApplyJump();

        _characterController.Move(_velocity * Time.deltaTime);
        //animations
        MovementAnimations();
    }

    private void ApplyJump()
    {
        if (_jump && _characterController.isGrounded)
        {
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
        }
    }

    private void LimitMaximumRunningSpeed()
    {
        Vector3 yVelocity = Vector3.Scale(_velocity, new Vector3(0, 1, 0));
        Vector3 xzVelocity = Vector3.Scale(_velocity, new Vector3(1, 0, 1));

        Vector3 clampedXzVelocity = Vector3.ClampMagnitude(xzVelocity, _maxRunningSpeed);

        _velocity = yVelocity + clampedXzVelocity;
    }

    private void ApplyGroundDrag()
    {
        if(_characterController.isGrounded)
        {
            _velocity = _velocity * (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void ApplyMovement()
    {
        if(_characterController.isGrounded)
        {
            Vector3 xzAbsoluteForward = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));

            Quaternion forwardRotation = Quaternion.LookRotation(xzAbsoluteForward);
            Vector3 relativeMovement = forwardRotation * _movement;

            _velocity += relativeMovement * _acceleration * Time.deltaTime;
        }
    }

    private void ApplyGround()
    {
        if(_characterController.isGrounded)
        {
            _velocity -= Vector3.Project(_velocity, Physics.gravity.normalized);
        }
    }

    private void ApplyGravity()
    {
        if(!_characterController.isGrounded)
        {
            _velocity += Physics.gravity * Time.deltaTime;
        }
    }

    private void MovementAnimations()
    {
        //left thumbstick input
        _animator.SetFloat(_horizontalVelocityParameter, _movement.x);
        _animator.SetFloat(_verticalVelocityParameter, _movement.z);
    }
}
