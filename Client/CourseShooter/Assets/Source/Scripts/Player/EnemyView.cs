using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(EnemyRotation))]
public class EnemyView : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private EnemyAnimation _enemyAnimation;
    private EnemyRotation _enemyRotation;

    private Vector3 _moveDirection = Vector3.zero;

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _enemyRotation = GetComponent<EnemyRotation>();
    }

    private void OnEnable()
    {
        _enemyMovement.GroundedStateChanged += OnGroundedChanged;
        _enemyMovement.MoveDirectionChanged += OnSpeedChanged;
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

        _enemyMovement.SetMoveData(targetPosition, _moveDirection);
    }

    public void OnRotationChange(List<DataChange> changes)
    {
        Vector3 targetRotation = transform.rotation.eulerAngles;
        targetRotation.x = _enemyRotation.HeadRotationX;

        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    targetRotation.x = (float)change.Value;
                    break;

                case "y":
                    targetRotation.y = (float)change.Value;
                    break;

                case "z":
                    targetRotation.z = (float)change.Value;
                    break;
            }
        }

        _enemyRotation.SetRotation(targetRotation);
    }

    private void OnGroundedChanged(bool state)
    {
        _enemyAnimation.SetGroundState(state);
    }

    private void OnSpeedChanged(Vector3 direction)
    {
        _enemyAnimation.SetMovementSpeed(direction.z);
    }
}
