using Colyseus;
using Colyseus.Schema;
using GameDevWare.Serialization;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : ColyseusManager<Test>
{
    private const string GameName = "state_handler";
    private const string LobbyName = "MyLobbyRoom";
    public ColyseusRoom<LobbyState> _lobby { get; private set; }
    private ColyseusRoom<State> _room;

    public event Action<int> PlayersCountChanged;

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

        _lobby.OnMessage<List<IndexedDictionary<string, object>>>("rooms", Rooms);
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
                    Debug.Log("PlayerChange" + change.Value);
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
        Debug.Log("Remove " + roomID);
    }

    private void OnRoomUpdate(List<object> objs)
    {
        string roomID = (string)objs[0];
        Debug.Log("RoomChanged");
        Debug.Log(roomID);
        IndexedDictionary<string, object> roomInfo = (IndexedDictionary<string, object>)objs[1];
        IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)roomInfo["metadata"];

        foreach (var data in roomInfo)
        {
            Debug.Log(data.Key + " || " + data.Value);
        }

        foreach (var data in metadata)
        {
            Debug.Log(data.Key + " || " + data.Value);
        }
    }
    private void Rooms(List<IndexedDictionary<string, object>> roomInfos)
    {
        Debug.Log("RoomInfo--------" + roomInfos.Count);
        foreach (var roomInfo in roomInfos)
        {
            Debug.Log(roomInfo);
            IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)roomInfo["metadata"];
            Debug.Log(metadata);
        }
    }
}
