using System;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private Vector3 _currentRotation;
    private float _sensetivity = 10;

    public event Action<Vector3> RotationXChanged;

    public void SetBehaviourState(bool state)
    {
        _camera.gameObject.SetActive(state);
    }

    public void Rotate()
    {
        //_camera.transform.localRotation = Quaternion.Euler(_currentRotation.x, 0, 0);
        //transform.rotation = Quaternion.Euler(0, _currentRotation.y, 0);

        _camera.transform.localEulerAngles = new Vector3(_currentRotation.x, 0, 0);
        transform.eulerAngles = new Vector3(0, _currentRotation.y, 0);
        _currentRotation.y = transform.localRotation.eulerAngles.y;

        RotationXChanged?.Invoke(_currentRotation);
    }

    public void AddRotationAxis(Vector3 axis)
    {
        _currentRotation.y += axis.x * _sensetivity;
        _currentRotation.x -= axis.y * _sensetivity;

        float maxRotateXAngle = 89;
        _currentRotation.x = Math.Clamp(_currentRotation.x, -maxRotateXAngle, maxRotateXAngle);
    }
}
