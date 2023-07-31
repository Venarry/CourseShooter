using Colyseus.Schema;
using System;
using System.Collections.Generic;
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

    public void SetMoveData(Vector3 newPosition, Vector3 moveDirection)
    {
        _newPosition = newPosition;
        _moveDirection = moveDirection;
    }

    private void InterpolateWithLerp()
    {
        float interpolationMultiplier = 0.25f;
        transform.position = Vector3.Lerp(transform.position, _newPosition, interpolationMultiplier);
    }

    private void InterpolateWithSmallPredicate()
    {
        float interpolationMultiplier = 0.25f;
        transform.position = Vector3.Lerp(transform.position, _newPosition + _moveDirection, interpolationMultiplier);
    }

    private void InterpolateWithPredicate()
    {
        float interpolationMultiplier = 0.25f;
        transform.position = Vector3.Lerp(transform.position, _newPosition, interpolationMultiplier); 
        _newPosition += _moveDirection;
    }

    private void InterpolateWithPredicateCharacterController()
    {
        float interpolationMultiplier = 0.4f;
        _newPosition += _moveDirection;
        _characterController.Move(_newPosition - Vector3.Lerp(transform.position, _newPosition, interpolationMultiplier));
    }
}
