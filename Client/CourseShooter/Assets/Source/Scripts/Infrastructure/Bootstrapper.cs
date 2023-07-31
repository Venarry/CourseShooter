using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private MultiplayerHolder _multiplayerHolder;

    private void Awake()
    {
        _multiplayerHolder.InitClient();
        _multiplayerHolder.JoinRoom();
    }

    private void OnDestroy()
    {
        _multiplayerHolder.LeaveRoom();
    }
}