using UnityEngine;

public class LookAtRotator : MonoBehaviour
{
    private Transform _target;

    /*private void Start()
    {
        _target = Camera.main.transform;
    }*/

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        transform.forward = _target.transform.forward * -1;
    }
}
