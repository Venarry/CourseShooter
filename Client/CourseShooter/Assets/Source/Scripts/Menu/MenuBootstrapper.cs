using TMPro;
using UnityEngine;

public class MenuBootstrapper : MonoBehaviour
{
    [SerializeField] private RoomsListView _roomsListView;
    [SerializeField] private OnlinePlayersShower _onlinePlayersShower;
    [SerializeField] private TMP_Text _version;

    private void Awake()
    {
        var lobbyHandler = LobbyRoomHandler.Instance;

        _roomsListView.Init(lobbyHandler);
        _onlinePlayersShower.Init(lobbyHandler);

        _version.text = $"Версия игры: {GameConfig.Version}";
    }
}
