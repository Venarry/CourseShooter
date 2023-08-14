using UnityEngine;

public class LookAtRotator : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null)
            return;

        transform.forward = _camera.transform.forward * -1;
    }
}
