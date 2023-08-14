using UnityEngine;

public class PistolWeapon : WeaponView
{
    [SerializeField] private Transform _shootPoint;
    private BulletsFactory _bulletsFactory;

    private void Awake()
    {
        _bulletsFactory = new();
    }

    public override bool TryShoot(OwnerData ownerData)
    {
        if (IsReadyToShoot == false)
            return false;

        _bulletsFactory.CreatePistolBullet(_shootPoint, Damage);
        ResetShootTime();
        return true;
    }
}
