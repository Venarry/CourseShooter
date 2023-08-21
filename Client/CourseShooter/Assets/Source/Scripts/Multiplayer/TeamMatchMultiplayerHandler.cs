using Colyseus;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class TeamMatchMultiplayerHandler : MonoBehaviour
{
    private ColyseusRoom<State> _room;
    private readonly Dictionary<string, EnemyView> _spawnedEnemys = new();
    private readonly Dictionary<string, Player> _joinedEnemys = new();
    private EnemyFactory _enemyFactory;
    private PlayerFactory _playerFactory;
    private ChatView _chatView;
    private TeamsDataSource _teamsDataSource;
    private MapScoreView _mapScoreView;

    public event Action<PlayerView> PlayerSpawned;

    public string SessionId => _room.SessionId;

    public void Init(ChatView chatView,
        MapScoreView mapScoreView,
        TeamsDataSource teamsDataSource,
        EnemyFactory enemyFactory,
        PlayerFactory playerFactory)
    {
        _enemyFactory = enemyFactory;
        _playerFactory = playerFactory;
        _chatView = chatView;
        _teamsDataSource = teamsDataSource;
        _mapScoreView = mapScoreView;
    }

    public void SendPlayerData(string key, object data = null)
    {
        _room.Send(key, data);
    }

    public void JoinRoom()
    {
        _room = StateHandlerRoom.Instance.Room;

        _room.State.players.OnAdd += OnPlayerAdd;
        _room.State.players.OnRemove += OnHeroRemove;
        _room.State.Score.OnAdd += _mapScoreView.OnScoreTeamAdd;

        _room.OnMessage<string>("Shoot", OnShoot);
        _room.OnMessage<string>("MessageSent", OnMessageSent);
        _room.OnMessage<string>("Respawn", OnRespawn);
    }

    private void OnRespawn(string id)
    {
        _spawnedEnemys[id].Respawn();
    }

    private void OnMessageSent(string message)
    {
        _chatView.SentMessage(message);
    }

    private void OnShoot(string data)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(data);

        string clientId = shootInfo.ShooterData.ClientId;
        if (_spawnedEnemys.ContainsKey(clientId) == false)
            return;

        _spawnedEnemys[clientId].Shoot(shootInfo);
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
        Vector3 spawnPosition = new(player.Position.x, player.Position.y, player.Position.z);
        int teamIndex = player.TeamIndex.ConvertTo<int>();

        var playerView = _playerFactory.Create(spawnPosition, teamIndex, true, player);
        _teamsDataSource.AddToTeam(playerView, teamIndex);

        PlayerSpawned?.Invoke(playerView);
    }

    private void CreateEnemy(string key, Player player)
    {
        _joinedEnemys.Add(key, player);
        SpawnEnemy(key);
    }

    private void SpawnEnemy(string key)
    {
        Player thisPlayer = _joinedEnemys[key];

        EnemyView enemy = _enemyFactory.Create(thisPlayer, key);
        _spawnedEnemys.Add(key, enemy);
        _teamsDataSource.AddToTeam(enemy, enemy.TeamIndex);
    }
}
