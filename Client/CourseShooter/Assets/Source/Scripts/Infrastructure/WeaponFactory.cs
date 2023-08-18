using UnityEngine;

public class WeaponFactory
{
    public WeaponView Create(string prefabPath, MainCameraHolder mainCameraHolder)
    {
        WeaponView weaponPrefab = Resources.Load<WeaponView>(prefabPath);
        WeaponView weapon = Object.Instantiate(weaponPrefab);
        weapon.Init(prefabPath, mainCameraHolder);

        return weapon;
    }
}
