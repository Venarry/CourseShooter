using UnityEngine;

public class MinigunWeapon : WeaponView
{
    [SerializeField] private Transform _shootPoint;
    private BulletsFactory _bulletsFactory;

    private void Awake()
    {
        _bulletsFactory = new();
    }

    public override bool TryShoot()
    {
        if (IsReadyToShoot == false)
            return false;

        _bulletsFactory.CreateSphereBullet(_shootPoint, Damage);
        ResetShootTime();
        return true;
    }
}
