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

    public Vector3 TargetPosition => _targetPosition;

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

        /*float teleportDistance = 2f;

        if (Vector3.Distance(transform.position, _targetPosition) > teleportDistance)
        {
            _characterController.enabled = false;
            transform.position = _targetPosition;
            _characterController.enabled = true;
        }*/
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetMoveDirection(Vector3 moveDirection)
    {
        _moveDirection = moveDirection;
    }

    private void InterpolateWithPredicateCharacterController()
    {
        float interpolationMultiplier = 0.4f;
        _targetPosition += _moveDirection;
        _characterController.Move(_targetPosition - Vector3.Lerp(transform.position, _targetPosition, interpolationMultiplier));
    }
}
