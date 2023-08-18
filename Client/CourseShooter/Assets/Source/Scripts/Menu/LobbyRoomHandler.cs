using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyRoomHandler : ColyseusManager<LobbyRoomHandler>
{
    private const string LobbyName = "MyLobbyRoom";

    public event Action<int> PlayersCountChanged;
    public event Action<IndexedDictionary<string, object>> RoomDataUpdated;
    public event Action<string> RoomRemoved;

    public ColyseusRoom<LobbyState> _lobby { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this)
            return;

        InitializeClient();
        DontDestroyOnLoad(gameObject);
        ConnectLobby();
    }

    private void OnDisable()
    {
        LeaveLobby();
    }

    private async void ConnectLobby()
    {
        _lobby = await client.JoinOrCreate<LobbyState>(LobbyName);
        _lobby.State.OnChange += OnStateDataChange;

        _lobby.OnMessage<List<IndexedDictionary<string, object>>>("rooms", AddRoomsRange);
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

            Debug.Log("data" + roomID);
        foreach (var data in mainData)
        {
            Debug.Log(data.Key + " || " + data.Value);
        }

        RoomDataUpdated?.Invoke(mainData);
    }

    private void AddRoomsRange(List<IndexedDictionary<string, object>> roomsInfo)
    {
        Debug.Log("RoomInfo--------" + roomsInfo.Count);
        foreach (IndexedDictionary<string, object> roomInfo in roomsInfo)
        {
            foreach (var item in roomInfo)
            {
                Debug.Log($"{item.Key} || {item.Value}");
            }
            IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)roomInfo["metadata"];

            RoomDataUpdated?.Invoke(roomInfo);
        }
    }
}
