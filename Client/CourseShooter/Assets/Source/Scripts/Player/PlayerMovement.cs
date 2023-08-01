using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _jumpStrength = 0.5f;
    [SerializeField] private float _gravity = 0.03f;

    private CharacterController _characterController;
    private IInputsHandler _inputsHandler;

    private Vector3 _moveDirection;
    private Vector3 _localVelocity;
    private float _gravityForce;
    private bool _isStayOnGound;

    public event Action<Vector3> DataChanged;

    public Vector3 Velocity => _moveDirection;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Init(IInputsHandler inputsHandler)
    {
        _inputsHandler = inputsHandler;
    }

    public void FixedUpdate()
    {
        Vector3 previousVelocity = Velocity;

        RefreshMoveDirection();
        _characterController.Move(_moveDirection);
        ReduceGravityForce();

        Vector3 currentVelocity = Velocity;

        if(previousVelocity != currentVelocity)
        {
            DataChanged?.Invoke(transform.position);
        }

        Debug.Log(Mathf.Sign(_moveDirection.normalized.z));
        Debug.Log(_moveDirection.magnitude);
    }

    private void Update()
    {
        TryJump();
    }

    public void RefreshMoveDirection()
    {
        _moveDirection = _inputsHandler.MoveDirection;
        _moveDirection = _moveDirection.normalized;
        _moveDirection *= _speed;
        _moveDirection += _localVelocity;
        _moveDirection.y = _gravityForce;

        _moveDirection = transform.forward * _moveDirection.z + transform.right * _moveDirection.x + transform.up * _moveDirection.y; 
    }

    public void TryJump()
    {
        if (_characterController.isGrounded && _inputsHandler.IsPressedKeyJump)
        {
            _gravityForce = _jumpStrength;
            _isStayOnGound = false;
        }
    }

    private void ReduceGravityForce()
    {
        float groudedGravity = -0.01f;

        if (_characterController.isGrounded)
        {
            _gravityForce = groudedGravity;
            _localVelocity = Vector3.zero;
            _isStayOnGound = true;
        }
        else
        {
            if (_isStayOnGound)
            {
                Vector3 moveDirection = _inputsHandler.MoveDirection;
                float stepSpeedMultiplier = 0.4f;

                _localVelocity += new Vector3(moveDirection.x * _speed * stepSpeedMultiplier, 0, moveDirection.z * _speed * stepSpeedMultiplier);
                _gravityForce = 0;
                _isStayOnGound = false;
            }

            _gravityForce -= _gravity;
        }


        if (_localVelocity.magnitude >= 0f)
        {
            _localVelocity -= _localVelocity * 0.02f;

            if (_localVelocity.magnitude < 0f)
                _localVelocity = Vector3.zero;
        }
    }
}
