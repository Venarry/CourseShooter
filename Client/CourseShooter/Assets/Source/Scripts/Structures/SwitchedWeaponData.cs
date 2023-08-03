public struct SwitchedWeaponData
{
    public string OwnerKey;
    public int WeaponIndex;

    public SwitchedWeaponData(string ownerKey, int weaponIndex)
    {
        OwnerKey = ownerKey;
        WeaponIndex = weaponIndex;
    }
}
