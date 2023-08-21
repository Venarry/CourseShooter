using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyRoomHandler : ColyseusManager<LobbyRoomHandler>
{
    private const string LobbyName = "MyLobbyRoom";

    public event Action<int> PlayersCountChanged;
    public event Action<IndexedDictionary<string, object>> RoomDataUpdated;
    public event Action<string> RoomRemoved;

    private ColyseusRoom<LobbyState> _lobby;
    private Dictionary<string, IndexedDictionary<string, object>> _rooms;

    public Dictionary<string, IndexedDictionary<string, object>> Rooms => _rooms.ToDictionary(room => room.Key, room => room.Value);

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;

        InitializeClient();
        DontDestroyOnLoad(gameObject);
        ConnectLobby();
        _rooms = new();
    }

    private void OnDisable()
    {
        LeaveLobby();
    }

    private async void ConnectLobby()
    {
        _lobby = await client.JoinOrCreate<LobbyState>(LobbyName);
        _lobby.State.OnChange += OnStateDataChange;

        _lobby.OnMessage<List<IndexedDictionary<string, object>>>("rooms", OnRoomsLoad);
        _lobby.OnMessage<List<object>>("+", OnRoomUpdate);
        _lobby.OnMessage<string>("-", OnRoomRemoved);
    }

    private void OnStateDataChange(List<DataChange> changes)
    {
        foreach (var change in changes)
        {
            switch (change.Field)
            {
                case "PlayersCount":
                    PlayersCountChanged?.Invoke(change.Value.ConvertTo<int>());
                    break;
            }
        }
    }

    public void LeaveLobby()
    {
        _lobby.Leave();
    }

    private void OnRoomRemoved(string roomID)
    {
        Debug.Log($"Removed {roomID}");
        RoomRemoved?.Invoke(roomID);
    }

    private void OnRoomUpdate(List<object> roomData)
    {
        string roomID = (string)roomData[0];
        IndexedDictionary<string, object> mainData = (IndexedDictionary<string, object>)roomData[1];
        IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)mainData["metadata"];

        /*Debug.Log("data" + roomID);
        foreach (var data in mainData)
        {
            Debug.Log(data.Key + " || " + data.Value);
        }*/

        RoomDataUpdated?.Invoke(mainData);
    }

    private void OnRoomsLoad(List<IndexedDictionary<string, object>> roomsInfo)
    {
        foreach (IndexedDictionary<string, object> roomInfo in roomsInfo)
        {
            foreach (var item in roomInfo)
            {
                Debug.Log($"{item.Key} || {item.Value}");
            }
            IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)roomInfo["metadata"];
            _rooms.Add((string)roomInfo["roomId"], roomInfo);
            RoomDataUpdated?.Invoke(roomInfo);
        }
    }
}
