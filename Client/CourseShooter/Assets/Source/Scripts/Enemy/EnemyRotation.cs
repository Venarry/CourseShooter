using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [SerializeField] private GameObject _head;

    public float HeadRotationX => _head.transform.localRotation.eulerAngles.x;

    public void SetRotation(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        _head.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
    }
}
