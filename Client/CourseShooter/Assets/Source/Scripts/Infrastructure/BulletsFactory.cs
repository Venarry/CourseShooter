using UnityEngine;

public class BulletsFactory
{
    private readonly SphereBullet _sphereBulletPrefab = Resources.Load<SphereBullet>(ResourcesPath.SphereBullet);

    public Bullet CreateSphereBullet(Transform parent, int damage)
    {
        SphereBullet bullet = Object.Instantiate(_sphereBulletPrefab);
        bullet.Init(parent, damage);

        return bullet;
    }
}
