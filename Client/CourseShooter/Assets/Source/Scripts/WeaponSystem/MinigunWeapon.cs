using UnityEngine;

public class MinigunWeapon : WeaponView
{
    [SerializeField] private Transform _shootPoint;
    private BulletsFactory _bulletsFactory;

    private void Awake()
    {
        _bulletsFactory = new();
    }

    public override bool TryShoot(ShootData ownerData)
    {
        if (IsReadyToShoot == false)
            return false;

        //_bulletsFactory.CreateSphereBullet(_shootPoint, Damage);
        float maxDistance = 100f;

        if(Physics.Raycast(_shootPoint.position, _shootPoint.transform.forward, out RaycastHit hitInfo, maxDistance) == true)
        {
            if(hitInfo.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage, ownerData);
            }
        }

        ResetShootTime();
        return true;
    }
}
