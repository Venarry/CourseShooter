using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour
{
    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _targetPosition = Vector3.zero;

    public event Action<bool> GroundedStateChanged;
    public event Action<Vector3> MoveDirectionChanged;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        bool startGroundState = _characterController.isGrounded;
        InterpolateWithPredicateCharacterController();

        if(startGroundState != _characterController.isGrounded)
        {
            GroundedStateChanged?.Invoke(_characterController.isGrounded);
        }

        MoveDirectionChanged?.Invoke(_characterController.velocity);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetMoveDirection(Vector3 moveDirection)
    {
        _moveDirection = moveDirection;
    }

    private void InterpolateWithSmallPredicate()
    {
        float interpolationMultiplier = 0.25f;
        transform.position = Vector3.Lerp(transform.position, _targetPosition + _moveDirection, interpolationMultiplier);
    }

    private void InterpolateWithPredicate()
    {
        float interpolationMultiplier = 0.25f;
        _targetPosition += _moveDirection;
        transform.position = Vector3.Lerp(transform.position, _targetPosition, interpolationMultiplier); 
    }

    private void InterpolateWithPredicateCharacterController()
    {
        float interpolationMultiplier = 0.4f;
        _targetPosition += _moveDirection;
        _characterController.Move(_targetPosition - Vector3.Lerp(transform.position, _targetPosition, interpolationMultiplier));
    }
}
