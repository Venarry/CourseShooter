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
    private float _gravityForce;

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
        }
            DataChanged?.Invoke(transform.position);
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
        _moveDirection = transform.forward * _moveDirection.z + transform.right * _moveDirection.x;
        _moveDirection.y = _gravityForce;
    }

    public void TryJump()
    {
        if (_characterController.isGrounded && _inputsHandler.IsPressedKeyJump)
        {
            _gravityForce = _jumpStrength;
        }
    }

    private void ReduceGravityForce()
    {
        float groudedGravity = -1f;

        if (_characterController.isGrounded)
            _gravityForce = groudedGravity;
        else
            _gravityForce -= _gravity;
    }
}
