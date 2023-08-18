using Colyseus;
using GameDevWare.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateHandlerRoom : ColyseusManager<StateHandlerRoom>
{
    private ColyseusRoom<State> _room;
    public ColyseusRoom<State> Room => _room;

    protected override void Awake()
    {
        base.Awake();

        InitializeClient();
        DontDestroyOnLoad(this);
    }

    public void SendPlayerData(string key, object data)
    {
        _room.Send(key, data);
    }

    public async void JoinOrCreate()
    {
        _room = await client.JoinOrCreate<State>("state_handler");
    }

    public async Task<bool> JoinRoomById(string id)
    {
        var rooms = await client.GetAvailableRooms();

        foreach (ColyseusRoomAvailable room in rooms)
        {
            if(room.roomId == id)
            {
                if (room.clients >= room.maxClients)
                {
                    return false;
                }

                _room = await client.JoinById<State>(id);
                

                return true;
            }
        }

        return false;
    }

    public async Task<bool> CreateRoom()
    {
        Dictionary<string, object> roomMetaData = new()
        {
            { "roomPassword", "123" },
        };

        _room = await client.Create<State>("state_handler", roomMetaData);
        return true;
    }

    public async Task<ColyseusRoomAvailable[]> GetRooms()
    {
        return await client.GetAvailableRooms();

        /*
        var rooms = await client.GetAvailableRooms();

        foreach (ColyseusRoomAvailable room in rooms)
        {
            Debug.Log($"name {room.name}, maxClient {room.maxClients}, id {room.roomId}");
        }
        */
    }

    public void Leave()
    {
        _room.Leave();
    }

    /*private void OnEnable()
    {
        _room.State.players.OnAdd += OnPlayerAdd;
        _room.State.players.OnRemove += OnHeroRemove;
        _room.State.Score.OnAdd += _mapScoreView.OnScoreTeamAdd;

        _room.OnMessage<string>("Shoot", OnShoot);
        _room.OnMessage<string>("MessageSent", OnMessageSent);
        _room.OnMessage<string>("SpawnPlayer", OnSpawnPlayer);
    }

    private void AddListenner()
    {
        _room.OnMessage<string>("Shoot", OnShoot);
        _room.OnMessage<string>("MessageSent", OnMessageSent);
        _room.OnMessage<string>("SpawnPlayer", OnSpawnPlayer);
    }*/
}
