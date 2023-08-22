using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScoreModel
{
    private readonly Dictionary<int, int> _teamsScore = new();

    public event Action<int, int> ScoreChanged;

    public int GetTeamScore(int teamIndex)
    {
        if (_teamsScore.ContainsKey(teamIndex) == false)
            return 0;

        return _teamsScore[teamIndex];
    }

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

    public void SetScore(int teamIndex, int value)
    {
        if (_teamsScore.ContainsKey(teamIndex) == false)
        {
            _teamsScore.Add(teamIndex, value);
        }
        else
        {
            _teamsScore[teamIndex] = value;
        }

        ScoreChanged?.Invoke(teamIndex, _teamsScore[teamIndex]);
    }

    public void Reset()
    {
        _teamsScore.Clear();
    }
}
