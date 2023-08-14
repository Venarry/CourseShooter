using Colyseus;
using UnityEngine;
using System.Collections.Generic;

public class MultiplayerHandler : ColyseusManager<MultiplayerHandler>
{
    private ColyseusRoom<State> _room;
    private readonly Dictionary<string, EnemyView> _enemys = new();
    private EnemyFactory _enemyFactory;
    private ChatView _chatView;
    private SpawnPointsDataSource _playerSpawner;
    private PlayerFactory _playerFactory;
    private Camera _diedFollowCamera;
    private MapScoreView _mapScoreView;

    private Vector3 _spawnPosition;
    private int _currentTeamIndex;

    public string SessionId => _room.SessionId;

    public void Init(ChatView chatView, SpawnPointsDataSource playerSpawner, Camera diedFollowCamera, MapScoreView mapScoreView)
    {
        _playerFactory = new();
        _enemyFactory = new();
        _chatView = chatView;
        _playerSpawner = playerSpawner;
        _diedFollowCamera = diedFollowCamera;
        _mapScoreView = mapScoreView;
    }

    public void InitClient()
    {
        Instance.InitializeClient();
    }

    public void SendPlayerData(string key, object data)
    {
        _room.Send(key, data);
    }

    public async void JoinRoom()
    {
        TeamsDataSource teamsDataSource = new(teamsCount: 2);

        _currentTeamIndex = Random.Range(0, 2); //teamsDataSource.AddPlayerToSmallestTeam();
        _spawnPosition = _playerSpawner.GetRandomSpawnPosition(_currentTeamIndex);

        Dictionary<string, object> startPlayerData = new()
        {
            { "Position", new MyVector3(_spawnPosition) },
            { "TeamIndex", _currentTeamIndex }
        };

        _room = await client.JoinOrCreate<State>("state_handler", startPlayerData);

        _room.State.players.OnAdd += SpawnHero;
        _room.State.players.OnRemove += RemoveHero;
        _room.State.Score.OnAdd += _mapScoreView.OnScoreAdd;

        _room.OnMessage<string>("Shoot", OnShoot);
        _room.OnMessage<string>("MessageSent", OnMessageSent);
    }

    private void OnMessageSent(string message)
    {
        _chatView.SentMessage(message);
    }

    private void OnShoot(string ownerId)
    {
        if (_enemys.ContainsKey(ownerId) == false)
            return;

        OwnerData ownerData = new(_enemys[ownerId].TeamIndex);
        _enemys[ownerId].Shoot(ownerData);
    }

    public void LeaveRoom()
    {
        _room.Leave();

        _room.State.players.OnAdd -= SpawnHero;
        _room.State.players.OnRemove -= RemoveHero;
    }

    private void SpawnHero(string key, Player player)
    {
        if (key == _room.SessionId)
        {
            SpawnPlayer(player);
        }
        else
        {
            SpawnEnemy(key, player);
        }
    }

    private void RemoveHero(string key, Player value)
    {
        Destroy(_enemys[key].gameObject);
        _enemys.Remove(key);
    }

    private void SpawnPlayer(Player player)
    {
        _playerFactory.Create(multiplayerHandler: this, _playerSpawner, _diedFollowCamera, _spawnPosition, _currentTeamIndex, isMultiplayer: true);
    }

    private void SpawnEnemy(string key, Player player)
    {
        EnemyView enemy = _enemyFactory.Create(player);
        _enemys.Add(key, enemy);
    }
}
