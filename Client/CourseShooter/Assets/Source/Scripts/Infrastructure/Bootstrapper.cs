using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private MultiplayerHandler _multiplayerHolder;
    [SerializeField] private ChatView _chatView;

    private void Awake()
    {
        _multiplayerHolder.Init(_chatView);
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
