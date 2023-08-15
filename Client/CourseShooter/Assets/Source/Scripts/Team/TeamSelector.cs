using UnityEngine;

public class TeamSelector : MonoBehaviour
{
    [SerializeField] private GameObject _selectionMenu;

    private PlayerView _playerView;
    private PlayerRespawner _playerRespawner;
    private PlayerFactory _playerFactory;

    public void Init(PlayerRespawner playerRespawner, PlayerFactory playerFactory)
    {
        _playerRespawner = playerRespawner;
        _playerFactory = playerFactory;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _selectionMenu.SetActive(_selectionMenu.activeInHierarchy == false);
        }
    }

    public void SetVisibility(bool state)
    {
        _selectionMenu.SetActive(state);
    }

    public void SelectTeam(int teamIndex)
    {
        if(_playerView == null)
        {
            _playerView = _playerFactory.Create(Vector3.zero, teamIndex, true);
            Vector3 respawnPosition = _playerRespawner.Respawn(_playerView);
            MultiplayerHandler.Instance.SendPlayerData("OnPlayerSpawn", respawnPosition);
            SetVisibility(false);
        }
        else
        {
            if(_playerView.TeamIndex != teamIndex)
            {
                _playerView.SetTeamIndex(teamIndex);
                _playerRespawner.Respawn(_playerView);
                SetVisibility(false);
            }
        }
    }
}
