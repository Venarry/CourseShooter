using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsStateHandler
{
    private readonly TeamsDataSource _teamsData;
    private readonly PlayerRespawner _playerRespawner;
    private readonly MapScoreView _mapScoreView;

    public TeamsStateHandler(TeamsDataSource teamsData, PlayerRespawner playerRespawner, MapScoreView mapScoreView)
    {
        _teamsData = teamsData;
        _playerRespawner = playerRespawner;
        _mapScoreView = mapScoreView;
    }

    public void Enable()
    {
        _teamsData.OneTeamAlived += OnRoundEnd;
    }

    public void OnRoundEnd(int winnerIndex)
    {
        _mapScoreView.AddScore(winnerIndex);

        Dictionary<string, object> data = new()
        {
            { "TeamIndex", winnerIndex },
            { "Value", _mapScoreView.GetTeamScore(winnerIndex) },
        };

        StateHandlerRoom.Instance.SendPlayerData("SetScore", data);
        _playerRespawner.Respawn();
        //StateHandlerRoom.Instance.SendPlayerData("StartRound");
    }
}
