using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _jumpStrength = 0.5f;
    [SerializeField] private float _gravity = 0.03f;

    private CharacterController _characterController;
    private Vector3 _inputAxis;
    private Vector3 _moveDirection;
    private Vector3 _localVelocity;
    private float _gravityForce;
    private bool _isStayOnGound;

    public event Action<Vector3> PositionChanged;

    public Vector3 MoveDirection => _moveDirection;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Move()
    {
        Vector3 previousVelocity = _characterController.velocity;
        _characterController.Move(_moveDirection);
        ReduceGravityForce();

        Vector3 currentVelocity = _characterController.velocity;

        if (previousVelocity != currentVelocity)
        {
            PositionChanged?.Invoke(transform.position);
        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        _inputAxis = direction;
        _moveDirection = direction;
        _moveDirection = _moveDirection.normalized;
        _moveDirection *= _speed;
        _moveDirection += _localVelocity;
        _moveDirection.y = _gravityForce;

        _moveDirection = transform.forward * _moveDirection.z + transform.right * _moveDirection.x + transform.up * _moveDirection.y; 
    }

    public void TryJump()
    {
        if (_characterController.isGrounded)
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
                float stepSpeedMultiplier = 0.4f;
                float targetSpeed = _speed * stepSpeedMultiplier;

                _localVelocity += new Vector3(_inputAxis.x * targetSpeed, 0, _inputAxis.z * targetSpeed);
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
