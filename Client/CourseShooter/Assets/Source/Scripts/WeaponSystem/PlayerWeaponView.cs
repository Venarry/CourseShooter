using System;
using UnityEngine;

public class PlayerWeaponView : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint;

    private PlayerWeaponPresenter _playerWeaponPresenter;
    private WeaponView _activeWeapon;
    private bool _isinitialized;

    public event Action<string> WeaponAdded;
    public event Action<int> WeaponSwitched;
    public event Action<ShootInfo> Shooted;

    public void Init(PlayerWeaponPresenter playerWeaponPresenter)
    {
        gameObject.SetActive(false);

        _playerWeaponPresenter = playerWeaponPresenter;
        _isinitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isinitialized == false)
            return;

        _playerWeaponPresenter.Enable();
        _playerWeaponPresenter.WeaponAdded += OnWeaponAdded;
        _playerWeaponPresenter.WeaponSwitched += OnWeaponSwitched;
    }

    private void OnDisable()
    {
        if (_isinitialized == false)
            return;

        _playerWeaponPresenter.Disable();
        _playerWeaponPresenter.WeaponAdded -= OnWeaponAdded;
        _playerWeaponPresenter.WeaponSwitched -= OnWeaponSwitched;
    }

    public void Shoot(ShootInfo shootInfo, bool useShootPoint)
    {
        if (_activeWeapon == null)
            return;

        if (_activeWeapon.TryShoot(shootInfo, useShootPoint) == false)
            return;

        Shooted?.Invoke(shootInfo);
    }

    public void AddWeapon(WeaponView weaponView, bool haveSwitch)
    {
        _playerWeaponPresenter.AddWeapon(weaponView, haveSwitch);
    }

    public void NextWeapon()
    {
        _playerWeaponPresenter.NextWeapon();
    }

    public void SwitchWeapon(int index)
    {
        _playerWeaponPresenter.SwitchWeapon(index);
    }

    private void OnWeaponSwitched(WeaponView weapon)
    {
        if(_activeWeapon != null)
            _activeWeapon.Hide();

        _activeWeapon = weapon;
        _activeWeapon.Show();

        WeaponSwitched?.Invoke(_playerWeaponPresenter.ActiveWeaponIndex);
    }

    private void OnWeaponAdded(WeaponView weapon)
    {
        weapon.SetParent(_holdPoint);
        WeaponAdded?.Invoke(weapon.PrefabPath);
    }
}
