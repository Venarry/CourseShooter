using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour
{
    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _newPosition = Vector3.zero;

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

    public void SetMovePosition(Vector3 newPosition)
    {
        _newPosition = newPosition;
    }

    public void SetMoveDirection(Vector3 moveDirection)
    {
        _moveDirection = moveDirection;
    }

    private void InterpolateWithSmallPredicate()
    {
        float interpolationMultiplier = 0.25f;
        transform.position = Vector3.Lerp(transform.position, _newPosition + _moveDirection, interpolationMultiplier);
    }

    private void InterpolateWithPredicate()
    {
        float interpolationMultiplier = 0.25f;
        _newPosition += _moveDirection;
        transform.position = Vector3.Lerp(transform.position, _newPosition, interpolationMultiplier); 
    }

    private void InterpolateWithPredicateCharacterController()
    {
        float interpolationMultiplier = 0.4f;
        _newPosition += _moveDirection;
        _characterController.Move(_newPosition - Vector3.Lerp(transform.position, _newPosition, interpolationMultiplier));
    }
}