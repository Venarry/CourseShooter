using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsStateHandler
{
    private readonly TeamsDataSource _teamsData;
    private readonly PlayerRespawner _playerRespawner;

    public TeamsStateHandler(TeamsDataSource teamsData, PlayerRespawner playerRespawner)
    {
        _teamsData = teamsData;
        _playerRespawner = playerRespawner;
    }

    public void Enable()
    {
        _teamsData.OneTeamAlived += OnRoundEnd;
    }

    public void OnRoundEnd(int winnerIndex)
    {
        StateHandlerRoom.Instance.SendPlayerData("AddScore", winnerIndex);
        _playerRespawner.Respawn();
    }
}
