using UnityEngine;

public class PistolWeapon : WeaponView
{
    [SerializeField] private Transform _shootPoint;
    private BulletsFactory _bulletsFactory;

    private void Awake()
    {
        _bulletsFactory = new();
    }

    public override bool TryShoot(ShooterData ownerData)
    {
        if (IsReadyToShoot == false)
            return false;

        _bulletsFactory.CreatePistolBullet(_shootPoint, Damage, ownerData);
        ResetShootTime();
        return true;
    }
}
