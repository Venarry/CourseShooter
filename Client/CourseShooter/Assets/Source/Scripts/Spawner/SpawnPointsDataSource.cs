using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPointsDataSource : MonoBehaviour
{
    [SerializeField] private List<TeamSpawnPointsData> _spawnPoints;

    private Transform _currentSpawnPoint;

    public Vector3 GetRandomSpawnPosition(int teamNumber)
    {
        Transform[] teamPoints = _spawnPoints[teamNumber].Points;

        _currentSpawnPoint = teamPoints[UnityEngine.Random.Range(0, teamPoints.Length)];
        return _currentSpawnPoint.position;
    }
}

[Serializable]
public class TeamSpawnPointsData
{
    [SerializeField] private Transform[] _spawnPoints;

    public Transform[] Points => _spawnPoints.ToArray();
}
