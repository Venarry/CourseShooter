using Colyseus;
using GameDevWare.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomsListView : MonoBehaviour
{
    [SerializeField] private RoomData _roomDataTemplate;
    [SerializeField] private Transform _contentPoint;

    private LobbyRoomHandler _lobbyRoomHandler;
    private StateHandlerRoom _stateHandlerRoom;
    private Dictionary<string, RoomData> _showedRooms;

    private void Awake()
    {
        _stateHandlerRoom = StateHandlerRoom.Instance;
        _showedRooms = new();
    }

    public void Init(LobbyRoomHandler lobbyRoomHandler)
    {
        _lobbyRoomHandler = lobbyRoomHandler;
    }

    private void OnEnable()
    {
        _lobbyRoomHandler.RoomDataUpdated += OnRoomDataUpdated;
        _lobbyRoomHandler.RoomRemoved += OnRoomRemoved;
    }

    private void OnDisable()
    {
        _lobbyRoomHandler.RoomDataUpdated -= OnRoomDataUpdated;
        _lobbyRoomHandler.RoomRemoved -= OnRoomRemoved;
    }

    private void OnRoomDataUpdated(IndexedDictionary<string, object> roomData)
    {
        IndexedDictionary<string, object> metadata = (IndexedDictionary<string, object>)roomData["metadata"];

        string roomId = (string)roomData["roomId"];
        int clients = roomData["clients"].ConvertTo<int>();
        int maxClients = roomData["maxClients"].ConvertTo<int>();
        string mapName = (string)metadata["MapName"];
        string version = (string)metadata["Version"];

        if (_showedRooms.ContainsKey(roomId) == false)
        {
            RoomData room = Instantiate(_roomDataTemplate, _contentPoint);
            room.Init(_stateHandlerRoom, roomId);
            _showedRooms.Add(roomId, room);
        }

        _showedRooms[roomId].SetRoomData(clients, maxClients, mapName, version);
    }

    private void OnRoomRemoved(string roomId)
    {
        if (_showedRooms.ContainsKey(roomId))
        {
            Destroy(_showedRooms[roomId].gameObject);
            _showedRooms.Remove(roomId);
        }
    }

    private void RemoveRooms()
    {
        foreach (var room in _showedRooms)
        {
            Destroy(room.Value.gameObject);
        }

        _showedRooms.Clear();
    }
}
