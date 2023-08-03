using UnityEngine;

public class WeaponFactory
{
    public WeaponView Create(string prefabPath)
    {
        WeaponView weaponPrefab = Resources.Load<WeaponView>(prefabPath);
        WeaponView weapon = Object.Instantiate(weaponPrefab);
        weapon.SetPath(prefabPath);

        return weapon;
    }
}
