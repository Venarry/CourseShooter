using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private TeamMatchMultiplayerHandler _multiplayerHandler;
    [SerializeField] private MapScoreView _mapScoreView;
    [SerializeField] private MainCameraHolder _cameraHolder;
    [SerializeField] private SpawnPointsDataSource _spawnPointsDataSource;
    [SerializeField] private TeamSelector _teamSelector;
    [SerializeField] private ChatView _chatView;
    [SerializeField] private Camera _observerCamera;

    private void Awake()
    {
        TeamsDataSource teamsDataSource = new(2);
        PlayerRespawner playerRespawner = new(_spawnPointsDataSource);
        TeamsStateHandler teamsStateHandler = new(teamsDataSource, playerRespawner, _mapScoreView);
        teamsStateHandler.Enable();

        MultiplayerPlayerLauncher multiplayerPlayerLauncher = new(_teamSelector, _multiplayerHandler);
        multiplayerPlayerLauncher.Enable();

        PlayerFactory playerFactory = new();
        playerFactory.SetPlayerData(_multiplayerHandler, playerRespawner, _cameraHolder, _observerCamera);

        EnemyFactory enemyFactory = new();
        enemyFactory.SetEnemyData(_cameraHolder, _multiplayerHandler);

        _teamSelector.Init(playerRespawner);
        _multiplayerHandler.Init(_chatView, _mapScoreView, teamsDataSource, enemyFactory, playerFactory);
        _cameraHolder.SetCamera(_observerCamera);
    }

    private void Start()
    {
        _multiplayerHandler.JoinRoom();
    }

    private void OnDestroy()
    {
        _multiplayerHandler.LeaveRoom();
    }
}
