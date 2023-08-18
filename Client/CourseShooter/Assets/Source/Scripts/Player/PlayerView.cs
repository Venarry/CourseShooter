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

    public event Action<Vector3> PositionChanged;
    public event Action<Vector3> DirectionChanged;
    public event Action<Vector3> RotationChanged;
    public event Action<string> WeaponAdded;
    public event Action<int> WeaponSwitched;
    public event Action<int> HealthChanged;
    public event Action<int> TeamIndexChanged;
    public event Action<ShooterData> Killed;
    public event Action<ShootInfo> Shooted;

    public int TeamIndex { get; private set; }
    public string Id { get; private set; }

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCameraRotation = GetComponent<PlayerRotation>();
        _playerWeaponView = GetComponent<PlayerWeaponView>();
        _progressBar = GetComponent<ProgressBar>();
        MapSettings.HideCursor();
    }

    public void Init(HealthPresenter healthPresenter, IInputsHandler inputsHandler, int teamNumber, string id)
    {
        gameObject.SetActive(false);

        _inputsHandler = inputsHandler;
        _healthPresenter = healthPresenter;
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
        _isInitialized = true;
        Id = id;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _healthPresenter.Enable();
        _playerMovement.PositionChanged += OnPositionChanged;
        _playerMovement.DirectionChanged += OnDirectionChanged;
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
        _playerMovement.PositionChanged -= OnPositionChanged;
        _playerMovement.DirectionChanged -= OnDirectionChanged;
        _playerCameraRotation.RotationXChanged -= OnRotationXChanged;
        _playerWeaponView.WeaponAdded -= OnWeaponAdded;
        _playerWeaponView.WeaponSwitched -= OnWeaponSwitched;
        _playerWeaponView.Shooted -= OnShoot;
        _healthPresenter.HealthChanged -= OnHealthChanged;
        _healthPresenter.HealthOver -= OnHealthOver;
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

        if(state == false)
        {
            PauseHandler.AddPauseLevel();
        }
        else
        {
            PauseHandler.RemovePauseLevel();
            _playerCameraRotation.ShowCamera();
        }
    }

    public void SetTeamIndex(int index)
    {
        TeamIndex = index;
        TeamIndexChanged?.Invoke(TeamIndex);
    }

    public void SetPosition(Vector3 respawnPosition)
    {
        _playerMovement.SetPosition(respawnPosition);
    }

    public void Respawn(Vector3 respawnPosition)
    {
        _healthPresenter.Restore();
        _playerMovement.SetPosition(respawnPosition);
    }

    public void TakeDamage(int value, ShooterData ownerData)
    {
        if (ownerData.TeamIndex == TeamIndex)
            return;

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
            ShooterData shooterData = new(TeamIndex, Id);
            ShootInfo shootInfo = new();
            shootInfo.SetShooterData(shooterData);


            _playerWeaponView.Shoot(shootInfo, true);
            return;
        }

        if (_inputsHandler.IsPressedNextWeapon == true)
        {
            _playerWeaponView.NextWeapon();
        }
    }

    private void OnPositionChanged(Vector3 position)
    {
        //MovementData movementData = new(position, _playerMovement.MoveDirection);
        PositionChanged?.Invoke(position);
    }

    private void OnDirectionChanged(Vector3 direction)
    {
        DirectionChanged?.Invoke(direction);
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

    private void OnShoot(ShootInfo shootInfo)
    {
        Shooted?.Invoke(shootInfo);
    }

    private void OnHealthOver(ShooterData ownerData)
    {
        Killed?.Invoke(ownerData);
    }

    private void OnHealthChanged()
    {
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
        HealthChanged?.Invoke(_healthPresenter.Health);
    }
}
