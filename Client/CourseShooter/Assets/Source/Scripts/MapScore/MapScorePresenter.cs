using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScorePresenter
{
    private readonly MapScoreModel _mapScoreModel;
    private readonly MapScoreView _mapScoreView;

    public MapScorePresenter(MapScoreModel mapScoreModel, MapScoreView mapScoreView)
    {
        _mapScoreModel = mapScoreModel;
        _mapScoreView = mapScoreView;
    }

    public void Enable()
    {
        _mapScoreModel.ScoreChanged += OnScoreChanged;
    }

    public void Disable()
    {
        _mapScoreModel.ScoreChanged -= OnScoreChanged;
    }

    public int GetTeamScore(int teamIndex) =>
        _mapScoreModel.GetTeamScore(teamIndex);

    public void AddScore(int teamIndex)
    {
        _mapScoreModel.AddScore(teamIndex);
    }

    public void SetScore(int teamIndex, int value)
    {
        _mapScoreModel.SetScore(teamIndex, value);
    }

    private void OnScoreChanged(int key, int value)
    {
        _mapScoreView.RefreshScore(key, value);
    }
}
