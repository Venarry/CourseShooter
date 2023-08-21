using UnityEngine;

public class PlayerRespawner
{
    private readonly SpawnPointsDataSource _spawnPointsData;
    private PlayerView _playerView;

    public PlayerRespawner(SpawnPointsDataSource spawnPointsData)
    {
        _spawnPointsData = spawnPointsData;
    }

    public Vector3 GetRandomPosition(int teamIndex) =>
        _spawnPointsData.GetRandomSpawnPosition(teamIndex);

    public void SetPlayer(PlayerView playerView)
    {
        _playerView = playerView;
    }

    public Vector3 Respawn()
    {
        Vector3 respawnPosition = _spawnPointsData.GetRandomSpawnPosition(_playerView.TeamIndex);
        _playerView.Respawn(respawnPosition);

        return respawnPosition;
    }
}
