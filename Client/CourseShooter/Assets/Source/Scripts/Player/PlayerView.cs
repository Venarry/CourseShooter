using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(PlayerWeaponView))]
[RequireComponent(typeof(ProgressBar))]
public class PlayerView : MonoBehaviour, IDamageable
{
    private PlayerMovement _playerMovement;
    private PlayerRotation _playerCameraRotation;
    private PlayerWeaponView _playerWeaponView;
    private HealthPresenter _healthPresenter;
    private ProgressBar _progressBar;
    private IInputsHandler _inputsHandler;
    private bool _isInitialized;

    public event Action<MovementData> MovementDataChanged;
    public event Action<Vector3> RotationChanged;
    public event Action<string> WeaponAdded;
    public event Action<int> WeaponSwitched;
    public event Action<int> HealthChanged;
    public event Action<int> TeamIndexChanged;
    public event Action<OwnerData> Killed;
    public event Action Shooted;

    public int TeamIndex { get; private set; }

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCameraRotation = GetComponent<PlayerRotation>();
        _playerWeaponView = GetComponent<PlayerWeaponView>();
        _progressBar = GetComponent<ProgressBar>();
        MapSettings.HideCursor();
    }

    public void Init(HealthPresenter healthPresenter, IInputsHandler inputsHandler, int teamNumber)
    {
        gameObject.SetActive(false);

        _inputsHandler = inputsHandler;
        _healthPresenter = healthPresenter;
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _healthPresenter.Enable();
        _playerMovement.PositionChanged += OnMovementDataChanged;
        _playerCameraRotation.RotationXChanged += OnRotationXChanged;
        _playerWeaponView.WeaponAdded += OnWeaponAdded;
        _playerWeaponView.WeaponSwitched += OnWeaponSwitched;
        _playerWeaponView.Shooted += OnShoot;
        _healthPresenter.HealthChanged += OnHealthChanged;
        _healthPresenter.HealthOver += OnHealthOver;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _healthPresenter.Disable();
        _playerMovement.PositionChanged -= OnMovementDataChanged;
        _playerCameraRotation.RotationXChanged -= OnRotationXChanged;
        _playerWeaponView.WeaponAdded -= OnWeaponAdded;
        _playerWeaponView.WeaponSwitched -= OnWeaponSwitched;
        _playerWeaponView.Shooted -= OnShoot;
        _healthPresenter.HealthChanged -= OnHealthChanged;
    }

    private void Update()
    {
        if (PauseHandler.IsPaused == true)
            return;

        ProcessJump();
        ProcessShooting();
    }

    private void FixedUpdate()
    {
        if (PauseHandler.IsPaused == false)
        {
            _playerMovement.SetMoveDirection(_inputsHandler.MoveDirection);
            _playerCameraRotation.AddRotationAxis(_inputsHandler.RotationDirection);
        }
        else
        {
            _playerMovement.SetMoveDirection(Vector3.zero);
            _playerCameraRotation.AddRotationAxis(Vector3.zero);
        }

        ProcessMovement();
        ProcessRotation();
    }

    public void SetBehaviourState(bool state)
    {
        _playerMovement.SetBehaviourState(state);
        _playerCameraRotation.SetBehaviourState(state);

        if(state == false)
            PauseHandler.AddPauseLevel();
        else
            PauseHandler.RemovePauseLevel();
    }

    public void SetTeamIndex(int index)
    {
        TeamIndex = index;
        TeamIndexChanged?.Invoke(TeamIndex);
    }

    public void Respawn(Vector3 respawnPosition)
    {
        _healthPresenter.Restore();
        _playerMovement.SetPosition(respawnPosition);
    }

    public void TakeDamage(int value, OwnerData ownerData)
    {
        _healthPresenter.TakeDamage(value, ownerData);
    }

    public void AddWeapon(WeaponView weaponView)
    {
        _playerWeaponView.AddWeapon(weaponView, haveSwitch: true);
    }

    private void ProcessJump()
    {
        if (_inputsHandler.IsPressedKeyJump == true)
            _playerMovement.TryJump();
    }

    private void ProcessMovement()
    {
        _playerMovement.Move();
    }

    private void ProcessRotation()
    {
        _playerCameraRotation.Rotate();
    }

    private void ProcessShooting()
    {
        if (_inputsHandler.IsPressedShoot == true)
        {
            OwnerData ownerData = new(TeamIndex);
            _playerWeaponView.Shoot(ownerData);
            return;
        }

        if (_inputsHandler.IsPressedNextWeapon == true)
        {
            _playerWeaponView.NextWeapon();
        }
    }

    private void OnMovementDataChanged(Vector3 position)
    {
        MovementData movementData = new(position, _playerMovement.MoveDirection);
        MovementDataChanged?.Invoke(movementData);
    }

    private void OnRotationXChanged(Vector3 rotation)
    {
        RotationChanged?.Invoke(rotation);
    }

    private void OnWeaponAdded(string prefabPath)
    {
        WeaponAdded?.Invoke(prefabPath);
    }

    private void OnWeaponSwitched(int weaponIndex)
    {
        WeaponSwitched?.Invoke(weaponIndex);
    }

    private void OnShoot()
    {
        Shooted?.Invoke();
    }

    private void OnHealthOver(OwnerData ownerData)
    {
        Killed?.Invoke(ownerData);
    }

    private void OnHealthChanged()
    {
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
        HealthChanged?.Invoke(_healthPresenter.Health);
    }
}
