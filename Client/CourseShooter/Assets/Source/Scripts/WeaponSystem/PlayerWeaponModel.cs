using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponModel
{
    private readonly List<WeaponView> _weapons = new();
    private int _activeWeaponIndex = 0;

    public event Action<WeaponView> WeaponAdded;
    public event Action<WeaponView> WeaponSwiched;

    public int ActiveWeaponIndex => _activeWeaponIndex;

    public void AddWeapon(WeaponView weapon, bool haveSwitch)
    {
        _weapons.Add(weapon);
        WeaponAdded?.Invoke(weapon);

        if (haveSwitch == true)
        {
            _activeWeaponIndex = _weapons.Count - 1;
            WeaponSwiched?.Invoke(weapon);
        }
        else
        {
            weapon.Hide();
        }
    }

    public void RemoveWeapon(WeaponView weapon)
    {
        if (_weapons.Contains(weapon) == false)
            return;

        _weapons.Remove(weapon);
    }

    public void NextWeapon()
    {
        if(_weapons.Count == 0)
            return;

        if(_activeWeaponIndex == _weapons.Count - 1)
        {
            _activeWeaponIndex = 0;
        }
        else
        {
            _activeWeaponIndex++;
        }

        WeaponSwiched?.Invoke(_weapons[_activeWeaponIndex]);
    }

    public void SwitchWeapon(int index)
    {
        if (index >= _weapons.Count)
            return;

        _activeWeaponIndex = index;
        WeaponSwiched?.Invoke(_weapons[_activeWeaponIndex]);
    }
}
