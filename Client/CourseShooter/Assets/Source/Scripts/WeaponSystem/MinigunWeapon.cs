using UnityEngine;

public class MinigunWeapon : WeaponView
{
    [SerializeField] private Transform _shootPoint;

    public override bool TryShoot(ShootInfo shootInfo, bool useShootPoint)
    {
        _shootPoint = GetMainCamera().transform;

        if (IsReadyToShoot == false)
            return false;

        if(useShootPoint == false)
        {
            Shoot(shootInfo);
        }
        else
        {
            Debug.Log($"_shootPoint.position {_shootPoint.position} _shootPoint.eulerAngles {_shootPoint.eulerAngles}");
            shootInfo.SetShootPoint(_shootPoint.position);
            //shootInfo.SetShootRotation(_shootPoint.eulerAngles);
            shootInfo.SetShootDirection(_shootPoint.forward - _shootPoint.position);
            Shoot(shootInfo);
        }

        return true;
    }

    private void Shoot(ShootInfo shootInfo)
    {
        float maxDistance = 100f;

        Debug.DrawRay(shootInfo.ShootPoint, (shootInfo.ShootPoint + shootInfo.ShootDirection) * maxDistance, Color.red, 1);

        if(Physics.Raycast(shootInfo.ShootPoint, shootInfo.ShootPoint + shootInfo.ShootDirection, out RaycastHit hitInfo, maxDistance) == true)
        {
            if(hitInfo.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage, shootInfo.ShooterData);
            }
        }

        ResetShootTime();
    }

    protected override void OnCameraChanged(Camera camera)
    {
        _shootPoint = camera.transform;
    }
}
