using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _multiplayerHolder;
    [SerializeField] private MapScoreView _mapScoreView;
    [SerializeField] private SpawnPointsDataSource _playerSpawner;
    [SerializeField] private ChatView _chatView;
    [SerializeField] private Camera _diedCamera;

    private void Awake()
    {
        _multiplayerHolder.Init(_chatView, _playerSpawner, _diedCamera, _mapScoreView);
    }

    private void Start()
    {
        _multiplayerHolder.InitClient();
        _multiplayerHolder.JoinRoom();
    }

    private void OnDestroy()
    {
        _multiplayerHolder.LeaveRoom();
    }
}
