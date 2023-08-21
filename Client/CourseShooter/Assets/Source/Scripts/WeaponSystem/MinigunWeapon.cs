using UnityEngine;

public class MinigunWeapon : WeaponView
{
    public override bool TryShoot(ShootInfo shootInfo, bool useShootPoint)
    {
        Transform shootPoint = GetMainCamera().transform;

        if (IsReadyToShoot == false)
            return false;

        if(useShootPoint == false)
        {
            Shoot(shootInfo);
        }
        else
        {
            shootInfo.SetShootPoint(shootPoint.position);
            shootInfo.SetShootDirection(shootPoint.forward - shootPoint.position);
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
}
