using UnityEngine;

public class PistolWeapon : WeaponView
{
    [SerializeField] private Transform _shootPoint;

    private BulletsFactory _bulletsFactory;
    private Transform _camera;

    private void Awake()
    {
        _bulletsFactory = new();
    }

    public override bool TryShoot(ShootInfo shootInfo, bool useShootPoint)
    {
        _camera = GetMainCamera().transform;

        if (IsReadyToShoot == false)
            return false;

        Vector3 endPoint = _shootPoint.position + _shootPoint.forward;

        if(Physics.Raycast(_camera.position, _camera.forward, out RaycastHit raycastHit, 100))
        {
            endPoint = raycastHit.point;
        }

        if (useShootPoint == true)
        {
            Quaternion shootRotation = Quaternion.LookRotation(endPoint - _shootPoint.position);
            shootInfo.SetShootPoint(_shootPoint.position);
            shootInfo.SetShootRotation(shootRotation.eulerAngles);
            _bulletsFactory.CreatePistolBullet(shootInfo.ShootPoint, shootInfo.ShootRotation, shootInfo.ShooterData, Damage);
        }
        else
        {
            _bulletsFactory.CreatePistolBullet(shootInfo.ShootPoint, shootInfo.ShootRotation, shootInfo.ShooterData, Damage);
        }

        ResetShootTime();
        return true;
    }

    protected override void OnCameraChanged(Camera camera)
    {
        _camera = camera.transform;
    }
}
