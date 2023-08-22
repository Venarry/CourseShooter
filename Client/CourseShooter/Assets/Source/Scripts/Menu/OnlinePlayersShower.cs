using TMPro;
using UnityEngine;

public class OnlinePlayersShower : MonoBehaviour
{
    [SerializeField] private TMP_Text _onlinePlayersLabel;
    private LobbyRoomHandler _lobbyRoomHandler;

    public void Init(LobbyRoomHandler lobbyRoomHandler)
    {
        _lobbyRoomHandler = lobbyRoomHandler;
    }

    private void OnEnable()
    {
        _lobbyRoomHandler.PlayersCountChanged += OnPlayersCountChanged;
    }

    private void OnDisable()
    {
        _lobbyRoomHandler.PlayersCountChanged -= OnPlayersCountChanged;
    }

    private void OnPlayersCountChanged(int count)
    {
        _onlinePlayersLabel.text = $"Игроков в сети: {count}";
    }
}
