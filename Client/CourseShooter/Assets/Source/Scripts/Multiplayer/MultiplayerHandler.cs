using Colyseus;
using UnityEngine;
using System.Collections.Generic;

public class MultiplayerHandler : ColyseusManager<MultiplayerHandler>
{
    private ColyseusRoom<State> _room;
    private readonly Dictionary<string, EnemyView> _enemys = new();
    private PlayerFactory _playerFactory;
    private EnemyFactory _enemyFactory;

    public string ClientId => _room.SessionId;

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

        //_room.OnMessage<string>("WeaponAdded", OnWeaponAdded);
        //_room.OnMessage<string>("WeaponSwitched", OnWeaponSwitched);
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
        _playerFactory.Create(new Vector3(player.Position.x, 0, player.Position.z));
    }

    private void SpawnEnemy(string key, Player player)
    {
        EnemyView enemy = _enemyFactory.Create(player);
        _enemys.Add(key, enemy);
    }
}
