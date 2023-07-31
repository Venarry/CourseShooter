using Colyseus;
using UnityEngine;
using System.Collections.Generic;

public class MultiplayerHolder : ColyseusManager<MultiplayerHolder>
{
    private ColyseusRoom<State> _room;
    private readonly Dictionary<string, EnemyView> _enemys = new();
    private PlayerFactory _playerFactory;
    private EnemyFactory _enemyFactory;

    public void InitClient()
    {
        Instance.InitializeClient();
    }

    public void SendPlayerPosition(string key, MovementData movementData)
    {
        _room.Send(key, movementData);
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
    }

    public void LeaveRoom()
    {
        _room.Leave();
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
        _playerFactory.Create(new Vector3(player.x, 0, player.z));
    }

    private void SpawnEnemy(string key, Player player)
    {
        EnemyView enemy = _enemyFactory.Create(new Vector3(player.x, 0, player.z));
        _enemys.Add(key, enemy);
        player.OnChange += enemy.OnChange;
        player.Rotation.OnChange += enemy.OnRotationChange;
    }
}
