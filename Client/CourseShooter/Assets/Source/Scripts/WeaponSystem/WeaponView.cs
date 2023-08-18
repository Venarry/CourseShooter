using UnityEngine;

public abstract class WeaponView : MonoBehaviour
{
    [SerializeField] protected int Damage;
    [SerializeField] private float _cooldown;
    private Transform _parent;
    private float _timeAfterShoot;
    private MainCameraHolder _mainCameraHolder;

    public string PrefabPath { get; private set; }
    protected bool IsReadyToShoot => _timeAfterShoot > _cooldown;

    public abstract bool TryShoot(ShootInfo shootInfo, bool useShootPoint);

    public void Update()
    {
        _timeAfterShoot += Time.deltaTime;

        /*if (_parent == null)
            return;

        transform.SetPositionAndRotation(_parent.position, _parent.rotation);*/
    }

    public void Init(string path, MainCameraHolder mainCameraHolder)
    {
        PrefabPath = path;
        _mainCameraHolder = mainCameraHolder;
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

    protected void Enbable()
    {
        _mainCameraHolder.CameraChanged += OnCameraChanged;
    }

    protected void Disable()
    {
        _mainCameraHolder.CameraChanged -= OnCameraChanged;
    }

    protected virtual void OnCameraChanged(Camera camera) { }

    protected Camera GetMainCamera() =>
        _mainCameraHolder.ActiveCamera;

    protected void ResetShootTime()
    {
        _timeAfterShoot = 0;
    }
}
