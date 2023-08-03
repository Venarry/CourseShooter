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

    private float _pingTimer;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCameraRotation = GetComponent<PlayerRotation>();
        _playerWeaponView = GetComponent<PlayerWeaponView>();
    }

    public void Init(MultiplayerHandler multiplayerHandler, PlayerWeaponPresenter playerWeaponPresenter, IInputsHandler inputsHandler)
    {
        gameObject.SetActive(false);

        _multiplayerHandler = multiplayerHandler;
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
        _pingTimer += Time.deltaTime;

        ProcessJump();
        ProcessShooting();

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddWeapon(new WeaponFactory().Create(ResourcesPath.Minigun));
        }
    }

    private void FixedUpdate()
    {
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
        _playerMovement.SetMoveDirection(_inputsHandler.MoveDirection);
        _playerMovement.Move();
    }
    private void ProcessRotation()
    {
        _playerCameraRotation.AddRotationAxis(_inputsHandler.RotationDirection);
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
        if (_pingTimer < 0f)
        {
            return;
        }

        MovementData movementData = new(position, _playerMovement.MoveDirection);
        MultiplayerHandler.Instance.SendPlayerData("Move", movementData);
        _pingTimer = 0;
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
