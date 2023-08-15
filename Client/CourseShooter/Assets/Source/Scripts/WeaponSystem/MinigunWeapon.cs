using UnityEngine;

public class MinigunWeapon : WeaponView
{
    [SerializeField] private Transform _shootPoint;

    public override bool TryShoot(ShooterData shooterData)
    {
        if (IsReadyToShoot == false)
            return false;

        float maxDistance = 100f;

        if(Physics.Raycast(_shootPoint.position, _shootPoint.transform.forward, out RaycastHit hitInfo, maxDistance) == true)
        {
            if(hitInfo.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage, shooterData);
            }
        }

        ResetShootTime();
        return true;
    }
}
