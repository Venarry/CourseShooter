using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponPresenter
{
    private readonly PlayerWeaponModel _playerWeaponModel;

    public event Action<WeaponView> WeaponAdded;
    public event Action<WeaponView> WeaponSwitched;

    public int ActiveWeaponIndex => _playerWeaponModel.ActiveWeaponIndex;

    public PlayerWeaponPresenter(PlayerWeaponModel playerWeaponModel)
    {
        _playerWeaponModel = playerWeaponModel;
    }

    public void Enable()
    {
        _playerWeaponModel.WeaponAdded += OnWeaponAdded;
        _playerWeaponModel.WeaponSwiched += OnWeaponSwitched;
    }

    public void Disable()
    {
        _playerWeaponModel.WeaponAdded -= OnWeaponAdded;
        _playerWeaponModel.WeaponSwiched -= OnWeaponSwitched;
    }

    public void AddWeapon(WeaponView weapon, bool haveSwitch)
    {
        _playerWeaponModel.AddWeapon(weapon, haveSwitch);
    }

    public void RemoveWeapon(WeaponView weapon)
    {
        _playerWeaponModel.RemoveWeapon(weapon);
    }

    public void NextWeapon()
    {
        _playerWeaponModel.NextWeapon();
    }

    public void SwitchWeapon(int index)
    {
        _playerWeaponModel.SwitchWeapon(index);
    }

    private void OnWeaponAdded(WeaponView weapon)
    {
        WeaponAdded?.Invoke(weapon);
    }

    private void OnWeaponSwitched(WeaponView weapon)
    {
        WeaponSwitched?.Invoke(weapon);
    }
}
