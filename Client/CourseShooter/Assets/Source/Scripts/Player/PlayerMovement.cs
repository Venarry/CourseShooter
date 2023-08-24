using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _jumpStrength = 0.5f;
    [SerializeField] private float _gravity = 0.03f;

    private CharacterController _characterController;
    private Vector3 _inputAxis;
    private Vector3 _moveDirection;
    private Vector3 _localVelocity;
    private float _gravityForce;
    private bool _isStayOnGound;

    public event Action<Vector3> PositionChanged;
    public event Action<Vector3> DirectionChanged;

    public Vector3 MoveDirection => _moveDirection * Time.deltaTime;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void SetBehaviourState(bool state)
    {
        _characterController.enabled = state;
    }

    public void SetPosition(Vector3 position)
    {
        _characterController.enabled = false;
        _characterController.transform.position = position;
        _characterController.enabled = true;

        PositionChanged?.Invoke(transform.position);
    }

    public void Move()
    {
        Vector3 previousVelocity = _characterController.velocity;
        Vector3 previousPosition = transform.position;
        _characterController.Move(MoveDirection);
        ReduceGravityForce();

        Vector3 currentVelocity = _characterController.velocity;
        Vector3 currentPosition = transform.position;

        if (previousPosition != currentPosition)
        {
            PositionChanged?.Invoke(currentPosition);
        }

        if(previousVelocity != currentVelocity)
        {
            DirectionChanged?.Invoke(MoveDirection);
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

            _gravityForce -= _gravity * Time.deltaTime;
        }


        if (_localVelocity.magnitude >= 0f)
        {
            _localVelocity -= _localVelocity * Time.deltaTime;

            if (_localVelocity.magnitude < 0f)
                _localVelocity = Vector3.zero;
        }
    }
}
