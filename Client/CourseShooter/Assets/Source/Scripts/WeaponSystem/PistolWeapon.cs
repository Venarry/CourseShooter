using UnityEngine;

public class PistolWeapon : WeaponView
{
    public override bool TryShoot()
    {
        Debug.Log("pistol shoot");
        return true;
    }
}
