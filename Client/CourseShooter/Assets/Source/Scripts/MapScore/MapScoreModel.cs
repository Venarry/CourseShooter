using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScoreModel
{
    private Dictionary<int, int> _teamsScore = new();

    public event Action<int, int> ScoreChanged;

    public void AddScore(int teamIndex)
    {
        if(_teamsScore.ContainsKey(teamIndex) == false)
        {
            _teamsScore.Add(teamIndex, 1);
        }
        else
        {
            _teamsScore[teamIndex]++;
        }

        ScoreChanged?.Invoke(teamIndex, _teamsScore[teamIndex]);
    }
}
