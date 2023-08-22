using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamsDataSource
{
    private readonly Dictionary<int, List<ITeamable>> _teams = new();

    public event Action<int> OneTeamAlived;

    public TeamsDataSource(int teamsCount)
    {
        int minTeams = 2;

        if (teamsCount < minTeams)
            teamsCount = minTeams;

        for (int i = 0; i < teamsCount; i++)
        {
            _teams.Add(i, new List<ITeamable>());
        }
    }

    public void AddToTeam(ITeamable teamable, int teamIndex)
    {
        if (_teams.ContainsKey(teamIndex) == false)
        {
            _teams.Add(teamIndex, new List<ITeamable>());
        }

        _teams[teamIndex].Add(teamable);
        teamable.SetTeamIndex(teamIndex);

        teamable.TeamChanged += OnTeamChanged;
        teamable.HealthOver += OnPlayerHealthOver;
        teamable.Leaved += OnLeaved;
    }

    private void OnPlayerHealthOver()
    {
        var aliveTeams = GetAliveTeams();

        if(aliveTeams.Count == 1)
        {
            OneTeamAlived?.Invoke(aliveTeams[0]);
        }
    }

    private void OnTeamChanged(int previousValue, ITeamable player)
    {
        _teams[previousValue].Remove(player);
        _teams[player.TeamIndex].Add(player);
    }

    public void OnLeaved(ITeamable teamable)
    {
        if (_teams.ContainsKey(teamable.TeamIndex) == false)
        {
            return;
        }

        _teams[teamable.TeamIndex].Remove(teamable);
        teamable.TeamChanged -= OnTeamChanged;
        teamable.HealthOver -= OnPlayerHealthOver;
        teamable.Leaved -= OnLeaved;
    }

    public int AddPlayerToSmallestTeam(ITeamable teamable)
    {
        if (_teams.Count == 0)
            throw new ArgumentException();

        int playersCount = int.MaxValue;
        int smallestTeamNumber = -1;

        foreach (var team in _teams)
        {
            if(team.Value.Count < playersCount)
            {
                smallestTeamNumber = team.Key;
                playersCount = team.Value.Count;
            }
        }

        _teams[smallestTeamNumber].Add(teamable);
        teamable.SetTeamIndex(smallestTeamNumber);

        return smallestTeamNumber;
    }

    private List<int> GetAliveTeams()
    {
        List<int> aliveTeams = new();

        foreach (var team in _teams)
        {
            int aliveCounter = team.Value.Where(player => player.IsAlive == true).Count();

            if (aliveCounter > 0)
            {
                aliveTeams.Add(team.Key);
            }
        }

        return aliveTeams;
    }
}
