using Colyseus.Schema;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMultiplayerHandler : MonoBehaviour
{
    private PlayerView _playerView;
    private Player _thisPlayer;
    private TeamMatchMultiplayerHandler _multiplayerHandler;

    private bool _isInitialized;

    private void Awake()
    {
        _playerView = GetComponent<PlayerView>();
    }

    public void Init(TeamMatchMultiplayerHandler multiplayerHandler, Player thisPlayer)
    {
        enabled = false;

        _multiplayerHandler = multiplayerHandler;
        _thisPlayer = thisPlayer;
        _isInitialized = true;

        enabled = true;
    }

    public void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _playerView.PositionChanged += OnPositionChanged;
        _playerView.DirectionChanged += OnDirectionChanged;
        _playerView.RotationChanged += OnRotationChanged;
        _playerView.WeaponAdded += OnWeaponAdded;
        _playerView.WeaponSwitched += OnWeaponSwitched;
        _playerView.Shooted += OnShooted;
        _playerView.HealthChanged += OnHealthChanged;
        _playerView.TeamIndexChanged += OnTeamIndexChanged;
        _playerView.Killed += OnKilled;
        _playerView.Respawned += OnRespawned;

        _thisPlayer.OnChange += OnDataChange;
    }

    private void OnDataChange(List<DataChange> changes)
    {
        foreach (var change in changes)
        {
            switch (change.Field)
            {
                case ("Health"):
                    _playerView.SetHealth(change.Value.ConvertTo<int>());
                    break;
            }
        }
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _playerView.PositionChanged -= OnPositionChanged;
        _playerView.DirectionChanged -= OnDirectionChanged;
        _playerView.RotationChanged -= OnRotationChanged;
        _playerView.WeaponAdded -= OnWeaponAdded;
        _playerView.WeaponSwitched -= OnWeaponSwitched;
        _playerView.Shooted -= OnShooted;
        _playerView.HealthChanged -= OnHealthChanged;
        _playerView.TeamIndexChanged -= OnTeamIndexChanged;
        _playerView.Killed -= OnKilled;
        _playerView.Respawned -= OnRespawned;

        _thisPlayer.OnChange -= OnDataChange;
    }

    private void OnRespawned()
    {
        _multiplayerHandler.SendPlayerData("OnRespawn", _multiplayerHandler.SessionId);
    }

    private void OnTeamIndexChanged(int index)
    {
        _multiplayerHandler.SendPlayerData("OnTeamIndexChanged", index);
    }

    private void OnKilled(ShooterData shootData)
    {
        //_multiplayerHandler.SendPlayerData("AddScore", shootData);
    }

    private void OnShooted(ShootInfo shootInfo)
    {
        string data = JsonUtility.ToJson(shootInfo);
        _multiplayerHandler.SendPlayerData("OnShoot", data);
    }

    private void OnWeaponSwitched(int weaponIndex)
    {
        _multiplayerHandler.SendPlayerData("SwitchWeapon", weaponIndex);
    }

    private void OnWeaponAdded(string prefabPath)
    {
        _multiplayerHandler.SendPlayerData("AddWeapon", prefabPath);
    }

    private void OnRotationChanged(Vector3 rotation)
    {
        _multiplayerHandler.SendPlayerData("Rotate", rotation);
    }

    private void OnPositionChanged(Vector3 position)
    {
        _multiplayerHandler.SendPlayerData("SetPosition", position);
    }

    private void OnDirectionChanged(Vector3 direction)
    {
        _multiplayerHandler.SendPlayerData("SetDirection", direction);
    }

    private void OnHealthChanged(int value)
    {
        _multiplayerHandler.SendPlayerData("OnHealthChanged", value);
    }
}
