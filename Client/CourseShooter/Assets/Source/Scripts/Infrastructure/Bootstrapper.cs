using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _multiplayerHandler;
    [SerializeField] private MapScoreView _mapScoreView;
    [SerializeField] private SpawnPointsDataSource _spawnPointsDataSource;
    [SerializeField] private TeamSelector _teamSelector;
    [SerializeField] private ChatView _chatView;
    [SerializeField] private Camera _diedCamera;

    private void Awake()
    {
        PlayerFactory playerFactory = new();
        playerFactory.SetPlayerData(_multiplayerHandler, _spawnPointsDataSource, _diedCamera);

        PlayerRespawner playerRespawner = new(_spawnPointsDataSource);

        _teamSelector.Init(playerRespawner, playerFactory);
        _multiplayerHandler.Init(_chatView, _spawnPointsDataSource, _mapScoreView, _teamSelector);
    }

    private void Start()
    {
        _multiplayerHandler.InitClient();
        _multiplayerHandler.JoinRoom();
    }

    private void OnDestroy()
    {
        _multiplayerHandler.LeaveRoom();
    }
}
