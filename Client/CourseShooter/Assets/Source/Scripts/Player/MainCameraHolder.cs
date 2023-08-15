using System;
using UnityEngine;

public class MainCameraHolder : MonoBehaviour
{
    public Camera ActiveCamera { get; private set; }

    public event Action<Camera> CameraChanged;

    public void SetCamera(Camera camera)
    {
        if(ActiveCamera != null)
        {
            ActiveCamera.gameObject.SetActive(false);
        }

        ActiveCamera = camera;
        ActiveCamera.gameObject.SetActive(true);
        CameraChanged?.Invoke(camera);
    }
}
