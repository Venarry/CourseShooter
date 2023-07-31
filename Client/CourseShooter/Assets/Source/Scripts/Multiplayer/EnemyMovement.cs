using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour
{
    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _newPosition;

    public event Action<bool> GroundedChanged;
    public event Action<Vector3> MoveDirectionChanged;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void OnChange(List<DataChange> dataChanges)
    {
        Vector3 targetPosition = transform.position;

        foreach (DataChange change in dataChanges)
        {
            switch (change.Field)
            {
                case "x":
                    targetPosition.x = (float)change.Value;
                    break;

                case "y":
                    targetPosition.y = (float)change.Value;
                    break;

                case "z":
                    targetPosition.z = (float)change.Value;
                    break;

                case "DirectionX":
                    _moveDirection.x = (float)change.Value;
                    break;

                case "DirectionY":
                    _moveDirection.y = (float)change.Value;
                    break;

                case "DirectionZ":
                    _moveDirection.z = (float)change.Value;
                    break;
            }
        }

        _newPosition = targetPosition;
    }

    private void FixedUpdate()
    {
        bool startGroundState = _characterController.isGrounded;
        InterpolateWithPredicateCharacterController();

        if(startGroundState != _characterController.isGrounded)
        {
        }
            GroundedChanged?.Invoke(_characterController.isGrounded);

        MoveDirectionChanged?.Invoke(_characterController.velocity);
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
