using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _multiplayerHandler;
    [SerializeField] private MapScoreView _mapScoreView;
    [SerializeField] private MainCameraHolder _cameraHolder;
    [SerializeField] private SpawnPointsDataSource _spawnPointsDataSource;
    [SerializeField] private TeamSelector _teamSelector;
    [SerializeField] private ChatView _chatView;
    [SerializeField] private Camera _observerCamera;

    private void Awake()
    {
        PlayerFactory playerFactory = new();
        playerFactory.SetPlayerData(_multiplayerHandler, _spawnPointsDataSource, _cameraHolder, _observerCamera);

        EnemyFactory enemyFactory = new();
        enemyFactory.SetEnemyData(_cameraHolder);

        PlayerRespawner playerRespawner = new(_spawnPointsDataSource);

        _teamSelector.Init(playerRespawner, playerFactory);
        _multiplayerHandler.Init(_chatView, _spawnPointsDataSource, _mapScoreView, _teamSelector, enemyFactory);
        _cameraHolder.SetCamera(_observerCamera);
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
