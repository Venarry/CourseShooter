using UnityEngine;

public class BulletsFactory
{
    private readonly SphereBullet _pistolBulletPrefab = Resources.Load<SphereBullet>(ResourcesPath.PistolBullet);

    public Bullet CreatePistolBullet(Vector3 shootPoint, Vector3 shootDirection, ShooterData shooterData, int damage)
    {
        SphereBullet bullet = Object.Instantiate(_pistolBulletPrefab);
        bullet.Init(shootPoint, shootDirection, shooterData, damage);

        return bullet;
    }
}
