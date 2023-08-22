using Colyseus.Schema;
using System;
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
public class EnemyView : MonoBehaviour, IDamageable, ITeamable
{
    private EnemyMovement _enemyMovement;
    private EnemyAnimation _enemyAnimation;
    private EnemyRotation _enemyRotation;
    private PlayerWeaponView _playerWeaponView;
    private EnemyHealthView _enemyHealthView;
    private Player _myPlayer;
    private MainCameraHolder _mainCameraHolder;
    private TeamMatchMultiplayerHandler _stateHandlerRoom;

    private Vector3 _moveDirection = Vector3.zero;
    private bool _isInitialized;
    private string _sessionId;

    public event Action<int, ITeamable> TeamChanged;
    public event Action HealthOver;
    public event Action<ITeamable> Leaved;

    public int TeamIndex { get; private set; }
    public bool IsAlive { get; private set; }

    private void Awake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _enemyRotation = GetComponent<EnemyRotation>();
        _playerWeaponView = GetComponent<PlayerWeaponView>();
        _enemyHealthView = GetComponent<EnemyHealthView>();
    }

    public void Init(Player myPlayer, MainCameraHolder mainCameraHolder, TeamMatchMultiplayerHandler stateHandlerRoom, string sessionId)
    {
        gameObject.SetActive(false);

        _myPlayer = myPlayer;
        _mainCameraHolder = mainCameraHolder;
        _stateHandlerRoom = stateHandlerRoom;
        _sessionId = sessionId;
        IsAlive = true;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _enemyMovement.GroundedStateChanged += OnGroundedChanged;
        _enemyMovement.MoveDirectionChanged += OnVelocityChanged;

        _enemyHealthView.HealthChanged += OnHealthChanged;
        _enemyHealthView.HealthOver += OnHealthOver;

        _myPlayer.Position.OnChange += OnPositionChange;
        _myPlayer.Direction.OnChange += OnDirectionChange;
        _myPlayer.Rotation.OnChange += OnRotationChange;
        _myPlayer.WeaponPaths.OnAdd += OnWeaponPathsAdded;
        _myPlayer.OnChange += OnDataChange;
    }

    private void OnHealthOver()
    {
        IsAlive = false;
        HealthOver?.Invoke();
    }

    private void OnHealthChanged(int health)
    {
        Dictionary<string, object> data = new()
        {
            { "Id", _sessionId },
            { "Value", health },
        };

        _stateHandlerRoom.SendPlayerData("OnEnemyHealthChanged", data);
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _enemyMovement.GroundedStateChanged -= OnGroundedChanged;
        _enemyMovement.MoveDirectionChanged -= OnVelocityChanged;

        _enemyHealthView.HealthChanged -= OnHealthChanged;
        _enemyHealthView.HealthOver -= OnHealthOver;

        _myPlayer.Position.OnChange -= OnPositionChange;
        _myPlayer.Direction.OnChange -= OnDirectionChange;
        _myPlayer.Rotation.OnChange -= OnRotationChange;
        _myPlayer.WeaponPaths.OnAdd -= OnWeaponPathsAdded;
        _myPlayer.OnChange -= OnDataChange;
    }

    private void OnDestroy()
    {
        Leaved?.Invoke(this);
    }

    public void TakeDamage(int value, ShooterData ownerData)
    {
        
        if (ownerData.TeamIndex == TeamIndex)
            return;

        _enemyHealthView.TakeDamage(value, ownerData);
    }

    public void Respawn()
    {
        IsAlive = true;
    }

    public void Shoot(ShootInfo shootInfo)
    {
        _playerWeaponView.Shoot(shootInfo, false);
    }

    public void SetTeamIndex(int index)
    {
        int previousTeam = TeamIndex;
        TeamIndex = index;
        TeamChanged?.Invoke(previousTeam, this);
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
                    SetTeamIndex(change.Value.ConvertTo<int>());
                    break;

                case "Health":
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
