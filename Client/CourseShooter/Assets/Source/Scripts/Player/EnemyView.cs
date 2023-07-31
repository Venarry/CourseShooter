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

    public void OnPositionChange(List<DataChange> changes)
    {
        Vector3 targetPosition = transform.position;
        targetPosition = ApplyDataChanges(changes, targetPosition);

        _enemyMovement.SetMovePosition(targetPosition);
    }

    public void OnDirectionChange(List<DataChange> changes)
    {
        _moveDirection = ApplyDataChanges(changes, _moveDirection);
        _enemyMovement.SetMoveDirection(_moveDirection);
    }

    public void OnRotationChange(List<DataChange> changes)
    {
        Vector3 targetRotation = transform.rotation.eulerAngles;
        targetRotation.x = _enemyRotation.HeadRotationX;
        targetRotation = ApplyDataChanges(changes, targetRotation);

        _enemyRotation.SetRotation(targetRotation);
    }

    private Vector3 ApplyDataChanges(List<DataChange> changes, Vector3 startValue)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    startValue.x = (float)change.Value;
                    break;

                case "y":
                    startValue.y = (float)change.Value;
                    break;

                case "z":
                    startValue.z = (float)change.Value;
                    break;
            }
        }

        return startValue;
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
