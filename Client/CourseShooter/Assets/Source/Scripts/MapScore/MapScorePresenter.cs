using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScorePresenter
{
    private MapScoreModel _mapScoreModel;
    private MapScoreView _mapScoreView;


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

    private void OnScoreChanged(int key, int value)
    {
        _mapScoreView.RefreshScore(key, value);
    }
}
