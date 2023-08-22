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

    private ColyseusRoom<LobbyState> _activeLobby;

    public event Action<int> PlayersCountChanged;
    private Dictionary<string, IndexedDictionary<string, object>> _rooms;

    public event Action<IndexedDictionary<string, object>> RoomDataUpdated;
    public event Action<string> RoomRemoved;

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

    public string GetRoomPasswordById(string lobbyId)
    {
        if (_rooms.ContainsKey(lobbyId) == false)
            return null;

        var metadata = (IndexedDictionary<string, object>)_rooms[lobbyId]["metadata"];
        return (string)metadata["Password"];
    }

    public string GetRoomVersionById(string lobbyId)
    {
        if (_rooms.ContainsKey(lobbyId) == false)
            return null;

        var metadata = (IndexedDictionary<string, object>)_rooms[lobbyId]["metadata"];
        return (string)metadata["Version"];
    }

    private async void ConnectLobby()
    {
        _activeLobby = await client.JoinOrCreate<LobbyState>(LobbyName);
        _activeLobby.State.OnChange += OnStateDataChange;

        _activeLobby.OnMessage<List<IndexedDictionary<string, object>>>("rooms", OnRoomsLoad);
        _activeLobby.OnMessage<List<object>>("+", OnRoomUpdate);
        _activeLobby.OnMessage<string>("-", OnRoomRemoved);
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
        _activeLobby.Leave();
    }

    private void OnRoomRemoved(string roomID)
    {
        Debug.Log($"Removed {roomID}");
        RoomRemoved?.Invoke(roomID);
    }

    private void OnRoomUpdate(List<object> roomData)
    {
        IndexedDictionary<string, object> mainData = (IndexedDictionary<string, object>)roomData[1];
        IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)mainData["metadata"];

        string roomId = (string)mainData["roomId"];

        if (_rooms.ContainsKey(roomId) == false)
        {
            _rooms.Add(roomId, mainData);
        }

        RoomDataUpdated?.Invoke(mainData);
    }

    private void OnRoomsLoad(List<IndexedDictionary<string, object>> roomsInfo)
    {
        foreach (IndexedDictionary<string, object> roomInfo in roomsInfo)
        {
            IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)roomInfo["metadata"];
            _rooms.Add((string)roomInfo["roomId"], roomInfo);
            Debug.Log((string)roomInfo["roomId"]);

            RoomDataUpdated?.Invoke(roomInfo);
        }
    }
}
