using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelector : MonoBehaviour
{
    [SerializeField] private GameObject _selectionMenu;

    private PlayerView _playerView;
    private PlayerRespawner _playerRespawner;

    public event Action<Dictionary<string, object>> PlayerLaunched;

    public void Init(PlayerRespawner playerRespawner)
    {
        _playerRespawner = playerRespawner;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetVisibility(_selectionMenu.activeInHierarchy == false);
        }
    }

    public void SetPlayer(PlayerView playerView)
    {
        _playerView = playerView;
    }

    public void SetVisibility(bool state)
    {
        _selectionMenu.SetActive(state);

        if(state == true)
            MapSettings.ShowCursor();
        else
            MapSettings.HideCursor();
    }

    public void SelectTeam(int teamIndex)
    {
        if(_playerView == null)
        {
            Vector3 respawnPosition = _playerRespawner.GetRandomPosition(teamIndex);

            Dictionary<string, object> data = new()
            {
                { "Position", respawnPosition },
                { "TeamIndex", teamIndex },
            };

            PlayerLaunched?.Invoke(data);
            SetVisibility(false);

            MapSettings.HideCursor();
        }
        else
        {
            if(_playerView.TeamIndex != teamIndex)
            {
                _playerView.SetTeamIndex(teamIndex);
                _playerRespawner.Respawn();
                SetVisibility(false);

                MapSettings.HideCursor();
            }
        }
    }
}
