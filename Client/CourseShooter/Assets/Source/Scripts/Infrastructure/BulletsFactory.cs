using UnityEngine;

public class BulletsFactory
{
    private readonly SphereBullet _sphereBulletPrefab = Resources.Load<SphereBullet>(ResourcesPath.SphereBullet);
    private readonly SphereBullet _pistolBulletPrefab = Resources.Load<SphereBullet>(ResourcesPath.PistolBullet);

    public Bullet CreateSphereBullet(Transform parent, int damage, ShooterData shooterData)
    {
        SphereBullet bullet = Object.Instantiate(_sphereBulletPrefab);
        bullet.Init(parent, damage, shooterData);

        return bullet;
    }

    public Bullet CreatePistolBullet(Transform parent, int damage, ShooterData shootData)
    {
        SphereBullet bullet = Object.Instantiate(_pistolBulletPrefab);
        bullet.Init(parent, damage, shootData);

        return bullet;
    }
}
