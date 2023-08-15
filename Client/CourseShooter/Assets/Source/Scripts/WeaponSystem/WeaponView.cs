using UnityEngine;

public abstract class WeaponView : MonoBehaviour
{
    [SerializeField] protected int Damage;
    [SerializeField] private float _cooldown;
    private Transform _parent;
    private float _timeAfterShoot;

    public string PrefabPath { get; private set; }
    protected bool IsReadyToShoot => _timeAfterShoot > _cooldown;

    public abstract bool TryShoot(ShooterData ownerData);

    public void Update()
    {
        _timeAfterShoot += Time.deltaTime;

        /*if (_parent == null)
            return;

        transform.SetPositionAndRotation(_parent.position, _parent.rotation);*/
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

    protected void ResetShootTime()
    {
        _timeAfterShoot = 0;
    }
}
