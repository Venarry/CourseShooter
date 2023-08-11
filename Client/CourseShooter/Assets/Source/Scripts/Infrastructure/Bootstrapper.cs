using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _multiplayerHolder;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private ChatView _chatView;

    private void Awake()
    {
        _multiplayerHolder.Init(_chatView, _playerSpawner);
        _playerSpawner.Init(_multiplayerHolder);
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
