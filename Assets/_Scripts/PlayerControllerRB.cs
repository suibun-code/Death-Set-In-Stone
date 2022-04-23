using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRB : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    //private CharacterController _controller;
    private Rigidbody _rigidBody;

    //Player fields
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _speedChangeRate;
    private float _speed;

    //Mouse
    [Header("Mouse")]
    [SerializeField] private float _sensitivity;

    //Rotation & Velocity
    [Header("Rotation & Velocity")]
    [Range(0.0f, 0.3f)]
    [SerializeField] private float _rotationSmoothTime;
    [SerializeField] private bool _rotateOnMove = true;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    //Grounded
    [Header("Grounded")]
    [SerializeField] private bool _grounded;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private float _groundedOffset = -0.14f;
    [SerializeField] private float _groundedRadius = 0.28f;

    //Input
    PlayerInputs _input;

    //Cinemachine
    [Header("Cinemachine")]
    [SerializeField] private GameObject _cinemachineCameraTarget;
    [SerializeField] private bool _lockCameraPosition = false;
    [SerializeField] private float _bottomClamp = 0.0f;
    [SerializeField] private float _topClamp = 70.0f;
    private const float _threshold = 0.01f;
    private float _cinemachineYaw;
    private float _cinemachinePitch;
    private float _cameraAngleOverride;

    //Jump
    [Header("Jump")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpTimeout = 0.5f;
    [SerializeField] private float _fallTimeout = 0.15f;
    [SerializeField] private float _jumpCooldown = 1f;
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private bool canJump = true;

    //Player properties
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float SprintSpeed { get { return _sprintSpeed; } set { _sprintSpeed = value; } }
    public float SpeedChangeRate { get { return _speedChangeRate; } set { _speedChangeRate = value; } }
    public float Sensitivity { get { return _sensitivity; } set { _sensitivity = value; } }
    public float RotationSmoothTime { get { return _rotationSmoothTime; } set { _rotationSmoothTime = value; } }
    public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float JumpTimeout { get { return _jumpTimeout; } set { _jumpTimeout = value; } }
    public float FallTimeout { get { return _fallTimeout; } set { _fallTimeout = value; } }
    public bool IsGrounded { get { return _grounded; } set { _grounded = value; } }

    private void Start()
    {
        //_controller = GetComponent<CharacterController>();
        _rigidBody = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputs>();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        RespawnPlayer.instance.playerSpawnPosition = transform.position;
    }

    private void Update()
    {
        Jump();
        GroundedCheck();
        //Move();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Jump()
    {
        if (IsGrounded)
        {
            _fallTimeoutDelta = FallTimeout;

            if (_input.jump && canJump)
            {

                _rigidBody.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
                canJump = false;
                StartCoroutine(JumpCooldown(_jumpCooldown));
            }

            if (_jumpTimeoutDelta >= 0.0f)
                _jumpTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            _jumpTimeoutDelta = JumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
                _fallTimeoutDelta -= Time.deltaTime;
        }

        _input.jump = false;
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
        IsGrounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);

        IsGrounded = true;
    }

    private void Move()
    {
        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

        if (_input.move == Vector2.zero)
            targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_rigidBody.velocity.x, 0.0f, _rigidBody.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.fixedDeltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            if (_rotateOnMove)
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        //_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        Vector3 tempVect = targetDirection.normalized * (_speed * Time.fixedDeltaTime);
        //_rigidBody.MovePosition(transform.position + tempVect);
        _rigidBody.AddForce(tempVect);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !_lockCameraPosition)
        {
            _cinemachineYaw += _input.look.x * Time.deltaTime * Sensitivity;
            _cinemachinePitch += _input.look.y * Time.deltaTime * Sensitivity;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineYaw = ClampAngle(_cinemachineYaw, float.MinValue, float.MaxValue);
        _cinemachinePitch = ClampAngle(_cinemachinePitch, _bottomClamp, _topClamp);

        // Cinemachine will follow this target
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachinePitch + _cameraAngleOverride, _cinemachineYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    IEnumerator JumpCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        canJump = true;
    }
}
