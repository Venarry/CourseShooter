using Colyseus.Schema;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(EnemyRotation))]
[RequireComponent(typeof(PlayerWeaponView))]
[RequireComponent(typeof(ProgressBar))]
[RequireComponent(typeof(EnemyHealthView))]
public class EnemyView : MonoBehaviour, IDamageable
{
    private EnemyMovement _enemyMovement;
    private EnemyAnimation _enemyAnimation;
    private EnemyRotation _enemyRotation;
    private PlayerWeaponView _playerWeaponView;
    private EnemyHealthView _enemyHealthView;
    private Player _myPlayer;
    private MainCameraHolder _mainCameraHolder;

    private Vector3 _moveDirection = Vector3.zero;
    private bool _isInitialized;

    public int TeamIndex { get; private set; }

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _enemyRotation = GetComponent<EnemyRotation>();
        _playerWeaponView = GetComponent<PlayerWeaponView>();
        _enemyHealthView = GetComponent<EnemyHealthView>();
    }

    public void Init(Player myPlayer, MainCameraHolder mainCameraHolder)
    {
        gameObject.SetActive(false);

        _myPlayer = myPlayer;
        _mainCameraHolder = mainCameraHolder;
        _isInitialized = true;

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
        _myPlayer.OnChange += OnDataChange;
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
        _myPlayer.OnChange -= OnDataChange;
    }

    public void TakeDamage(int value, ShooterData ownerData) { }

    public void Shoot(ShootInfo shootInfo)
    {
        _playerWeaponView.Shoot(shootInfo, false);
    }

    public void SetTeamindex(int index)
    {
        TeamIndex = index;
    }

    public void SetMovePosition(Vector3 targetPosition)
    {
        _enemyMovement.SetTargetPosition(targetPosition);
    }

    private void OnPositionChange(List<DataChange> changes)
    {
        Vector3 targetPosition = _enemyMovement.TargetPosition;
        targetPosition = ApplyVectorChanges(changes, targetPosition);
        _enemyMovement.SetTargetPosition(targetPosition);
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
        _playerWeaponView.AddWeapon(new WeaponFactory().Create(value, _mainCameraHolder), haveSwith);
    }

    private void OnDataChange(List<DataChange> changes)
    {
        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "ActiveWeapon":
                    _playerWeaponView.SwitchWeapon(change.Value.ConvertTo<int>());
                    break;

                case "TeamIndex":
                    SetTeamindex(change.Value.ConvertTo<int>());
                    break;

                case "Health":
                    //_healthPresenter.SetHealth(change.Value.ConvertTo<int>());
                    _enemyHealthView.SetHealth(change.Value.ConvertTo<int>());
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
