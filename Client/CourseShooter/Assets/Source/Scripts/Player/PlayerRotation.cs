using System;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private MainCameraHolder _cameraHolder;
    private Vector3 _currentRotation;
    private float _sensetivity = 1;

    public event Action<Vector3> RotationXChanged;

    public void Init(MainCameraHolder cameraHolder)
    {
        _cameraHolder = cameraHolder;
        _cameraHolder.SetCamera(_camera);
    }

    public void ShowCamera()
    {
        _cameraHolder.SetCamera(_camera);
    }

    public void Rotate()
    {
        _camera.transform.localEulerAngles = new Vector3(_currentRotation.x, 0, 0);
        transform.eulerAngles = new Vector3(0, _currentRotation.y, 0);
        _currentRotation.y = transform.localRotation.eulerAngles.y;

        RotationXChanged?.Invoke(_currentRotation);
    }

    public void AddRotationAxis(Vector3 axis)
    {
        Debug.Log(axis);
        _currentRotation.y += axis.x * _sensetivity;
        _currentRotation.x -= axis.y * _sensetivity;

        float maxRotateXAngle = 89;
        _currentRotation.x = Math.Clamp(_currentRotation.x, -maxRotateXAngle, maxRotateXAngle);
    }
}
