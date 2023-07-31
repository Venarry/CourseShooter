using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimation))]
public class EnemyView : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private EnemyAnimation _enemyAnimation;

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
    }

    private void OnEnable()
    {
        _enemyMovement.GroundedChanged += OnGroundedChanged;
        _enemyMovement.MoveDirectionChanged += OnSpeedChanged;
    }

    public void OnChange(List<DataChange> dataChanges)
    {
        _enemyMovement.OnChange(dataChanges);
    }

    private void OnGroundedChanged(bool state)
    {
        _enemyAnimation.SetGround(state);
    }

    private void OnSpeedChanged(Vector3 direction)
    {
        _enemyAnimation.SetMovementSpeed(direction.z);
    }
}
