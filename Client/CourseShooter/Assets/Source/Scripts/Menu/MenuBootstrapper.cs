using UnityEngine;

public class MenuBootstrapper : MonoBehaviour
{
    [SerializeField] private RoomsListView _roomsListView;

    private void Awake()
    {
        var lobbyHandler = LobbyRoomHandler.Instance;

        _roomsListView.Init(lobbyHandler);
    }

    private void Start()
    {
        //_stateHandlerRoom.InitializeClient();
    }
}
