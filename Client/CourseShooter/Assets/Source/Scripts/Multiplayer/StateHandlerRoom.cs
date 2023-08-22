using Colyseus;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StateHandlerRoom : ColyseusManager<StateHandlerRoom>
{
    private ColyseusRoom<State> _room;
    private LobbyRoomHandler _lobbyRoomHandler;
    public ColyseusRoom<State> Room => _room;

    protected override void Awake()
    {
        base.Awake();

        InitializeClient();
        DontDestroyOnLoad(this);
        _lobbyRoomHandler = LobbyRoomHandler.Instance;
    }

    public void SendPlayerData(string key, object data = null)
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

                Dictionary<string, object> data = new()
                {
                    { "Version", GameConfig.Version },
                };

                string roomVersion = _lobbyRoomHandler.GetRoomVersionById(id);

                if (GameConfig.Version != roomVersion)
                    return false;

                _room = await client.JoinById<State>(id);

                return true;
            }
        }

        return false;
    }

    public async Task<bool> CreateRoom(string mapName)
    {
        Dictionary<string, object> roomMetaData = new()
        {
            { "RoomName", "MyRoom" },
            { "MapName", mapName },
            { "Password", "123" },
            { "Version", GameConfig.Version },
        };

        _room = await client.Create<State>("state_handler", roomMetaData);
        return true;
    }

    public async Task<ColyseusRoomAvailable[]> GetRooms()
    {
        return await client.GetAvailableRooms();
    }

    public void Leave()
    {
        _room.Leave();
    }
}
