using Colyseus;
using UnityEngine;
using System.Collections.Generic;
using System;

public class MultiplayerHandler : ColyseusManager<MultiplayerHandler>
{
    private ColyseusRoom<State> _room;
    private readonly Dictionary<string, EnemyView> _enemys = new();
    private PlayerFactory _playerFactory;
    private EnemyFactory _enemyFactory;
    private ChatView _chatView;
    private PlayerSpawner _playerSpawner;

    public string ClientId => _room.SessionId;

    public void Init(ChatView chatView, PlayerSpawner playerSpawner)
    {
        _chatView = chatView;
        _playerSpawner = playerSpawner;
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
        _playerFactory = new();
        _enemyFactory = new();

        _room = await client.JoinOrCreate<State>("state_handler");

        _room.State.players.OnAdd += SpawnHero;
        _room.State.players.OnRemove += RemoveHero;

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

        _enemys[ownerId].Shoot();
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
        Vector3 spawnPosition = new(player.Position.x, player.Position.y, player.Position.z);
        //_playerFactory.Create(this, spawnPosition);
        _playerSpawner.SpawnPlayer((int)player.SpawnPointIndex);
    }

    private void SpawnEnemy(string key, Player player)
    {
        //EnemyView enemy = _enemyFactory.Create(player);
        EnemyView enemy = _playerSpawner.SpawnEnemy(player);
        _enemys.Add(key, enemy);
    }
}
