using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    private Vector2 _newRotation;
    private Vector2 _targetRotation;

    public float HeadRotationX => _head.transform.localRotation.eulerAngles.x;

    private void Update()
    {
        float lerpValue = 0.4f;
        _targetRotation.x = Mathf.LerpAngle(_targetRotation.x, _newRotation.x, lerpValue);
        _targetRotation.y = Mathf.LerpAngle(_targetRotation.y, _newRotation.y, lerpValue);

        transform.eulerAngles = new Vector3(0, _targetRotation.y, 0);
        _head.transform.localEulerAngles = new Vector3(_targetRotation.x, 0, 0);
    }

    public void SetRotation(Vector3 rotation)
    {
        _newRotation.x = rotation.x;
        _newRotation.y = rotation.y;
    }
}
