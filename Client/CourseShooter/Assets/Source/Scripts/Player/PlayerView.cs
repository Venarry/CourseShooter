using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
[RequireComponent(typeof(PlayerWeaponView))]
public class PlayerView : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerRotation _playerCameraRotation;
    private PlayerWeaponView _playerWeaponView;
    private MultiplayerHandler _multiplayerHandler;
    private IInputsHandler _inputsHandler;
    private bool _isInitialized;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCameraRotation = GetComponent<PlayerRotation>();
        _playerWeaponView = GetComponent<PlayerWeaponView>();
        MapSettings.HideCursor();
    }

    public void Init(MultiplayerHandler multiplayerHandler, PlayerWeaponPresenter playerWeaponPresenter, IInputsHandler inputsHandler)
    {
        gameObject.SetActive(false);

        _multiplayerHandler = multiplayerHandler; // не нужно
        _inputsHandler = inputsHandler;
        _isInitialized = true;
        _playerWeaponView.Init(playerWeaponPresenter);

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _playerMovement.PositionChanged += OnMovementDataChanged;
        _playerCameraRotation.RotationXChanged += OnRotationXChanged;
        _playerWeaponView.WeaponAdded += OnWeaponAdded;
        _playerWeaponView.WeaponSwitched += OnWeaponSwitched;
        _playerWeaponView.Shooted += OnShoot;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _playerMovement.PositionChanged -= OnMovementDataChanged;
        _playerCameraRotation.RotationXChanged -= OnRotationXChanged;
        _playerWeaponView.WeaponAdded -= OnWeaponAdded;
        _playerWeaponView.WeaponSwitched -= OnWeaponSwitched;
        _playerWeaponView.Shooted -= OnShoot;
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
            _playerWeaponView.Shoot();
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
        MultiplayerHandler.Instance.SendPlayerData("Move", movementData);
    }

    private void OnRotationXChanged(Vector3 rotation)
    {
        _multiplayerHandler.SendPlayerData("Rotate", rotation);
    }

    private void OnWeaponAdded(string prefabPath)
    {
        _multiplayerHandler.SendPlayerData("AddWeapon", prefabPath);
    }

    private void OnWeaponSwitched(int weaponIndex)
    {
        _multiplayerHandler.SendPlayerData("SwitchWeapon", weaponIndex);
    }

    private void OnShoot()
    {
        _multiplayerHandler.SendPlayerData("OnShoot", _multiplayerHandler.ClientId);
    }
}
