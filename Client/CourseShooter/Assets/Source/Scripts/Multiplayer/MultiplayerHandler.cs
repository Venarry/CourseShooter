using Colyseus;
using UnityEngine;
using System.Collections.Generic;

public class MultiplayerHandler : ColyseusManager<MultiplayerHandler>
{
    private ColyseusRoom<State> _room;
    private readonly Dictionary<string, EnemyView> _spawnedEnemys = new();
    private readonly Dictionary<string, Player> _joinedEnemys = new();
    private EnemyFactory _enemyFactory;
    private ChatView _chatView;
    private SpawnPointsDataSource _spawnPointsDataSource;
    private TeamSelector _teamSelector;
    private MapScoreView _mapScoreView;

    public string SessionId => _room.SessionId;

    public void Init(ChatView chatView, 
        SpawnPointsDataSource spawnPointsDataSource,
        MapScoreView mapScoreView,
        TeamSelector teamSelector,
        EnemyFactory enemyFactory)
    {
        _enemyFactory = enemyFactory;
        _chatView = chatView;
        _teamSelector = teamSelector;
        _spawnPointsDataSource = spawnPointsDataSource;
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
        _room = await client.JoinOrCreate<State>("state_handler");

        _room.State.players.OnAdd += OnPlayerAdd;
        _room.State.players.OnRemove += OnHeroRemove;
        _room.State.Score.OnAdd += _mapScoreView.OnScoreTeamAdd;

        _room.OnMessage<string>("Shoot", OnShoot);
        _room.OnMessage<string>("MessageSent", OnMessageSent);
        _room.OnMessage<string>("SpawnPlayer", OnSpawnPlayer);
    }

    private void OnMessageSent(string message)
    {
        _chatView.SentMessage(message);
    }

    private void OnShoot(string ownerId)
    {
        if (_spawnedEnemys.ContainsKey(ownerId) == false)
            return;

        ShooterData ownerData = new(_spawnedEnemys[ownerId].TeamIndex);
        _spawnedEnemys[ownerId].Shoot(ownerData);
    }

    public void LeaveRoom()
    {
        _room.Leave();

        _room.State.players.OnAdd -= OnPlayerAdd;
        _room.State.players.OnRemove -= OnHeroRemove;
    }

    private void OnPlayerAdd(string key, Player player)
    {
        if (key == _room.SessionId)
        {
            SpawnPlayer(player);
        }
        else
        {
            CreateEnemy(key, player);
        }
    }

    private void OnHeroRemove(string key, Player value)
    {
        if (_spawnedEnemys.ContainsKey(key) == true)
            Destroy(_spawnedEnemys[key].gameObject);

        _spawnedEnemys.Remove(key);
        _joinedEnemys.Remove(key);
    }

    private void SpawnPlayer(Player player)
    {
        _teamSelector.SetVisibility(true);
    }

    private void CreateEnemy(string key, Player player)
    {
        _joinedEnemys.Add(key, player);

        if(player.IsSpawned)
        {
            OnSpawnPlayer(key);
        }
    }

    private void OnSpawnPlayer(string key)
    {
        Player thisPlayer = _joinedEnemys[key];

        EnemyView enemy = _enemyFactory.Create(thisPlayer);
        _spawnedEnemys.Add(key, enemy);
    }
}
