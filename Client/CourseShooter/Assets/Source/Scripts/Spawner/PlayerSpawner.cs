using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    private PlayerFactory _playerFactory;
    private EnemyFactory _enemyFactory;
    private MultiplayerHandler _multiplayerHandler;

    public void Init(MultiplayerHandler multiplayerHandler)
    {
        _multiplayerHandler = multiplayerHandler;
        _playerFactory = new PlayerFactory();
        _enemyFactory = new EnemyFactory();
    }

    public void SpawnPlayer(int pointIndex)
    {
        Transform targetSpawnPoint = _spawnPoints[pointIndex];
        _playerFactory.Create(_multiplayerHandler, targetSpawnPoint.position);
    }

    public EnemyView SpawnEnemy(Player thisPlayer)
    {
        Transform targetSpawnPoint = _spawnPoints[(int)thisPlayer.SpawnPointIndex];
        EnemyView enemyView = _enemyFactory.Create(thisPlayer, targetSpawnPoint.position);

        return enemyView;
    }
}
