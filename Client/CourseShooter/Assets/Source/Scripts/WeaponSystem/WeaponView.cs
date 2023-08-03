using UnityEngine;

public abstract class WeaponView : MonoBehaviour
{
    [SerializeField] protected int Damage;
    private Transform _parent;

    public string PrefabPath { get; private set; }

    public abstract void Shoot();

    public void Update()
    {
        if (_parent == null)
            return;

        //transform.SetPositionAndRotation(_parent.position, _parent.rotation);
    }

    public void SetPath(string path)
    {
        PrefabPath = path;
    }

    public void SetParent(Transform parent)
    {
        _parent = parent;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
