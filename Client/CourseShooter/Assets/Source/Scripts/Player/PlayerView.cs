using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(PlayerWeaponView))]
[RequireComponent(typeof(ProgressBar))]
public class PlayerView : MonoBehaviour, IDamageable, ITeamable
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
    public event Action<int, ITeamable> TeamChanged;
    public event Action<int> TeamIndexChanged;
    public event Action<ShooterData> Killed;
    public event Action HealthOver;
    public event Action Respawned;
    public event Action<ShootInfo> Shooted;
    public event Action<ITeamable> Leaved;

    public int TeamIndex { get; private set; }
    public bool IsAlive { get; private set; }
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
        //SetTeamIndex(teamNumber);
        Id = id;
        IsAlive = true;
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
        _isInitialized = true;

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
        _healthPresenter.HealthSet += OnHealthSet;
        _healthPresenter.HealthOver += OnHealthOver;
        _healthPresenter.Killed += OnKilled;
    }

    private void OnHealthOver()
    {
        IsAlive = false;
        HealthOver?.Invoke();
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
        _healthPresenter.HealthSet -= OnHealthSet;
        _healthPresenter.HealthOver -= OnHealthOver;
        _healthPresenter.Killed -= OnKilled;
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

    public void SetHealth(int value)
    {
        _healthPresenter.SetHealth(value);
    }

    public void SetTeamIndex(int index)
    {
        int previousTeam = TeamIndex;
        TeamIndex = index;
        TeamIndexChanged?.Invoke(TeamIndex);
        TeamChanged?.Invoke(previousTeam, this);
    }

    public void SetPosition(Vector3 respawnPosition)
    {
        _playerMovement.SetPosition(respawnPosition);
    }

    public void Respawn(Vector3 respawnPosition)
    {
        _healthPresenter.Restore();
        _playerMovement.SetPosition(respawnPosition);
        IsAlive = true;
        Respawned?.Invoke();
    }

    public void TakeDamage(int value, ShooterData ownerData) { }

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

    private void OnKilled(ShooterData ownerData)
    {
        IsAlive = false;
        Killed?.Invoke(ownerData);
    }

    private void OnHealthChanged()
    {
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
        HealthChanged?.Invoke(_healthPresenter.Health);
    }

    private void OnHealthSet()
    {
        _progressBar.SetValue(_healthPresenter.HealthNormalized);
    }
}
