using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsDataSource
{
    private readonly Dictionary<int, int> _teams = new();

    public TeamsDataSource(int teamsCount)
    {
        int minTeams = 2;

        if (teamsCount < minTeams)
            teamsCount = minTeams;

        for (int i = 0; i < teamsCount; i++)
        {
            _teams.Add(i, 0);
        }
    }

    public int AddPlayerToSmallestTeam()
    {
        if (_teams.Count == 0)
            throw new ArgumentException();

        int playersCount = int.MaxValue;
        int smallestTeamNumber = -1;

        foreach (var item in _teams)
        {
            if(item.Value < playersCount)
            {
                smallestTeamNumber = item.Key;
                playersCount = item.Value;
            }
        }

        _teams[smallestTeamNumber]++;

        return smallestTeamNumber;
    }
}
