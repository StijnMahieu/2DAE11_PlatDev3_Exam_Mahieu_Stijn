using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class CharacterControlScript : MonoBehaviour
{
    //animations
    private Animator _animator;
    //charactercontroller
    private CharacterController _characterController;

    [SerializeField]
    private GameObject _playerCamera;

    //lives
    public int _hitPoints = 3;

    //states
    public enum States
    {
        normalMode, //alive, normal behaviour
        holdingRock, //while rock picked up, no jumping
        pushingBox, //while pushing box, no jumping or rotating
        crouched,  //while crouched, no jumping, slowed movement
        dead  //after being shot 3 times, respawn at start
    }
    public States State = States.normalMode;

    //gameObjects
    [SerializeField]
    private GameObject _camPivot;

    //velocity
    private Vector3 _velocity = Vector3.zero;

    //movement
    private Vector3 _movement = Vector3.zero;

    //jump
    private bool _jump;
    [SerializeField]
    private float _jumpHeight;

    //LocomotionParameters
    [SerializeField]
    private float _mass = 75;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _dragOnGround;
    [SerializeField]
    private float _maxRunningSpeed;

    //Cameramultiplier
    private float _cameraMultiplier = 2;

    //Dependencies
    [SerializeField]
    private Transform _absoluteForward;

    //animation
    private int _horizontalVelocityParameter = Animator.StringToHash("HorizontalVelocity");
    private int _verticalVelocityParameter = Animator.StringToHash("VerticalVelocity");
    private bool _isJumping;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //statecheck
        print(State);

        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if(Input.GetButtonDown("Jump") && !_isJumping && State == States.normalMode)
        {
            _jump = true;
        }

        //right trigger rotation
        if (State != States.pushingBox)
        {
            RotateCamera();
        }
        switch (State)
        {
            case States.normalMode:
                
                break;

            case States.holdingRock:
                _jump = false;
                break;

            case States.pushingBox:
                _jump = false;
                _cameraMultiplier = 0;
                _camPivot.transform.localEulerAngles = Vector3.Scale(_camPivot.transform.localEulerAngles, new Vector3(0, 1, 1));
                _movement = new Vector3(0, 0, Input.GetAxis("Vertical"));
                break;

            case States.crouched:
                _jump = false;
                break;

            case States.dead:
                SceneManager.LoadScene(0);
                break;
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
        if (_jump && _characterController.isGrounded && State == States.normalMode)
        {
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
            _animator.SetTrigger("Jump");
            _isJumping = true;
        }
        else if (_characterController.isGrounded)
        {
            _isJumping = false;
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

    public void RotateCamera()
    {
        Vector3 tempRot = transform.localEulerAngles;
        tempRot.y += Input.GetAxis("HorizontalCam") *_cameraMultiplier;
        transform.localEulerAngles = tempRot;

        Vector3 rotationCamPivot = _camPivot.transform.localEulerAngles;
        rotationCamPivot.x += Input.GetAxis("VerticalCam") * -_cameraMultiplier;
        if (_playerCamera.gameObject.activeSelf)
        {
            rotationCamPivot.x = ClampAngle(rotationCamPivot.x, -20, 40);
        }
        else
        {
            rotationCamPivot.x = ClampAngle(rotationCamPivot.x, -40, 10);
        }
        _camPivot.transform.localEulerAngles = rotationCamPivot;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        if (max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }

    private void OnTriggerEnter(Collider _collision)
    {
        if (_collision.gameObject.tag == "DeadZoneTrigger")
        {
            State = States.dead;
        }
    }
}
