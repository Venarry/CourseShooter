using UnityEngine;

public class PlayerRespawner
{
    private readonly SpawnPointsDataSource _spawnPointsData;

    public PlayerRespawner(SpawnPointsDataSource spawnPointsData)
    {
        _spawnPointsData = spawnPointsData;
    }

    public Vector3 GetRandomPosition(int teamIndex) =>
        _spawnPointsData.GetRandomSpawnPosition(teamIndex);

    public Vector3 Respawn(PlayerView player)
    {
        Vector3 respawnPosition = _spawnPointsData.GetRandomSpawnPosition(player.TeamIndex);
        player.Respawn(respawnPosition);

        return respawnPosition;
    }
}
