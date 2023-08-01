using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCameraRotation))]
public class PlayerView : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerCameraRotation _playerCameraRotation;

    private bool _isInitialized;

    private float _pingTimer;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCameraRotation = GetComponent<PlayerCameraRotation>();
    }

    public void Init(IInputsHandler inputsHandler)
    {
        gameObject.SetActive(false);

        _playerMovement.Init(inputsHandler);
        _playerCameraRotation.Init(inputsHandler);
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _playerMovement.DataChanged += OnPositionChanged;
        _playerCameraRotation.RotationXChanged += OnRotationXChanged;
    }

    private void Update()
    {
        _pingTimer += Time.deltaTime;
    }

    private void OnPositionChanged(Vector3 position)
    {
        /*if (_pingTimer < 0.4f)
        {
            return;
        }*/

        MovementData movementData = new(position, _playerMovement.Velocity);
        MultiplayerHandler.Instance.SendPlayerPosition("Move", movementData);
        _pingTimer = 0;
    }
    private void OnRotationXChanged(Vector3 rotation)
    {
        //_head.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        MultiplayerHandler.Instance.SendPlayerData("Rotate", rotation);
    }
}
