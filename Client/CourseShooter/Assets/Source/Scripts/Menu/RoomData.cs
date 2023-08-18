using Colyseus;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [SerializeField] private Button _roomButton;
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_Text _currentClientCount;
    [SerializeField] private TMP_Text _maxClientCount;

    private StateHandlerRoom _stateHandlerRoom;
    private string _roomId;

    public void Init(StateHandlerRoom stateHandlerRoom, string roomId)
    {
        _stateHandlerRoom = stateHandlerRoom;
        _roomId = roomId;
    }

    public void SetRoomData(int clientCount, int maxClientCount)
    {
        _currentClientCount.text = clientCount.ToString();
        _maxClientCount.text = maxClientCount.ToString();
    }

    private void OnEnable()
    {
        _roomButton.onClick.AddListener(OnRoomButtonClick);
    }

    private void OnDisable()
    {
        _roomButton.onClick.RemoveListener(OnRoomButtonClick);
    }

    private async void OnRoomButtonClick()
    {
        if (await _stateHandlerRoom.JoinRoomById(_roomId) == false)
            return;

        SceneManager.LoadScene("Level1");
    }
}
