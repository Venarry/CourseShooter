using System;
using UnityEngine;

public class PlayerCameraRotation : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private IInputsHandler _inputsHandler;
    private Vector3 _currentRotation;
    private float _sensetivity = 10;

    public event Action<Vector3> RotationXChanged;

    public void Init(IInputsHandler inputsHandler)
    {
        _inputsHandler = inputsHandler;
    }

    private void FixedUpdate()
    {
        AddRotationAxis();

        _camera.transform.localRotation = Quaternion.Euler(_currentRotation.x, 0, 0);
        transform.rotation = Quaternion.Euler(0, _currentRotation.y, 0);

        RotationXChanged?.Invoke(_currentRotation);
    }

    public void AddRotationAxis()
    {
        _currentRotation.y += _inputsHandler.RotationDirection.x * _sensetivity;
        _currentRotation.x -= _inputsHandler.RotationDirection.y * _sensetivity;

        float maxRotateXAngle = 89;
        _currentRotation.x = Math.Clamp(_currentRotation.x, -maxRotateXAngle, maxRotateXAngle);
    }
}
