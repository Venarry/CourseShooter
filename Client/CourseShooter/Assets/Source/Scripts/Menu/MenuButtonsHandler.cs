using Colyseus;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonsHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField _roomIdLabel;
    [SerializeField] private RoomsListView _roomsListView;

    private StateHandlerRoom _stateHandlerRoom;

    private void Awake()
    {
        _stateHandlerRoom = StateHandlerRoom.Instance;
    }

    public async void JoinRoom()
    {
        if (await _stateHandlerRoom.JoinRoomById(_roomIdLabel.text) == false)
            return;

        SceneManager.LoadScene("Level1");
    }

    public async void CreateRoom(string mapName)
    {
        if (await _stateHandlerRoom.CreateRoom(mapName) == false)
            return;

        SceneManager.LoadScene(mapName);
    }
}
