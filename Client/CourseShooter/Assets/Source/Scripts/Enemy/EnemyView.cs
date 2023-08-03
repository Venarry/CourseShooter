using Colyseus.Schema;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(EnemyRotation))]
[RequireComponent(typeof(PlayerWeaponView))]
public class EnemyView : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private EnemyAnimation _enemyAnimation;
    private EnemyRotation _enemyRotation;
    private PlayerWeaponView _playerWeaponView;
    private Player _myPlayer;

    private Vector3 _moveDirection = Vector3.zero;
    private bool _isInitialized;

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _enemyRotation = GetComponent<EnemyRotation>();
        _playerWeaponView = GetComponent<PlayerWeaponView>();
    }

    public void Init(PlayerWeaponPresenter playerWeaponPresenter, Player myPlayer)
    {
        gameObject.SetActive(false);

        _myPlayer = myPlayer;
        _isInitialized = true;
        _playerWeaponView.Init(playerWeaponPresenter);

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _enemyMovement.GroundedStateChanged += OnGroundedChanged;
        _enemyMovement.MoveDirectionChanged += OnVelocityChanged;

        _myPlayer.Position.OnChange += OnPositionChange;
        _myPlayer.Direction.OnChange += OnDirectionChange;
        _myPlayer.Rotation.OnChange += OnRotationChange;
        _myPlayer.WeaponPaths.OnAdd += OnWeaponPathsAdded;
        _myPlayer.OnChange += OnActiveWeaponChange;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _enemyMovement.GroundedStateChanged -= OnGroundedChanged;
        _enemyMovement.MoveDirectionChanged -= OnVelocityChanged;

        _myPlayer.Position.OnChange -= OnPositionChange;
        _myPlayer.Direction.OnChange -= OnDirectionChange;
        _myPlayer.Rotation.OnChange -= OnRotationChange;
        _myPlayer.WeaponPaths.OnAdd -= OnWeaponPathsAdded;
        _myPlayer.OnChange -= OnActiveWeaponChange;
    }

    private void OnPositionChange(List<DataChange> changes)
    {
        Vector3 targetPosition = transform.position;
        targetPosition = ApplyVectorChanges(changes, targetPosition);

        _enemyMovement.SetMovePosition(targetPosition);
    }

    private void OnDirectionChange(List<DataChange> changes)
    {
        _moveDirection = ApplyVectorChanges(changes, _moveDirection);
        _enemyMovement.SetMoveDirection(_moveDirection);
    }

    private void OnGroundedChanged(bool state)
    {
        _enemyAnimation.SetGroundState(state);
    }

    private void OnVelocityChanged(Vector3 velocity)
    {
        float sign = Mathf.Sign(velocity.z);
        _enemyAnimation.SetMovementSpeed(velocity.magnitude * sign);
    }

    private void OnRotationChange(List<DataChange> changes)
    {
        Vector3 targetRotation = transform.rotation.eulerAngles;
        targetRotation.x = _enemyRotation.HeadRotationX;
        targetRotation = ApplyVectorChanges(changes, targetRotation);

        _enemyRotation.SetRotation(targetRotation);
    }

    private void OnWeaponPathsAdded(int key, string value)
    {
        bool haveSwith = _myPlayer.ActiveWeapon == key;
        _playerWeaponView.AddWeapon(new WeaponFactory().Create(value), haveSwith);
    }

    private void OnActiveWeaponChange(List<DataChange> changes)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "ActiveWeapon":
                    _playerWeaponView.SwitchWeapon(change.Value.ConvertTo<int>());
                    break;
            }
        }
    }

    private Vector3 ApplyVectorChanges(List<DataChange> changes, Vector3 startValue)
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
}
