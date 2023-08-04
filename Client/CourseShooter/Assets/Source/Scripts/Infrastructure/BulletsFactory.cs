using UnityEngine;

public class BulletsFactory
{
    private readonly SphereBullet _sphereBulletPrefab = Resources.Load<SphereBullet>(ResourcesPath.SphereBullet);
    private readonly SphereBullet _pistolBulletPrefab = Resources.Load<SphereBullet>(ResourcesPath.PistolBullet);

    public Bullet CreateSphereBullet(Transform parent, int damage)
    {
        SphereBullet bullet = Object.Instantiate(_sphereBulletPrefab);
        bullet.Init(parent, damage);

        return bullet;
    }

    public Bullet CreatePistolBullet(Transform parent, int damage)
    {
        SphereBullet bullet = Object.Instantiate(_pistolBulletPrefab);
        bullet.Init(parent, damage);

        return bullet;
    }
}
