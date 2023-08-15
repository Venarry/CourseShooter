using Colyseus.Schema;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MapScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _teamsScore;

    private MapScorePresenter _presenter;

    private void Awake()
    {
        _presenter = new(new MapScoreModel(), this);
    }

    public void OnScoreAdd(string key, MapScoreData value)
    {
        if (int.TryParse(key, out int teamIndex) == false)
            return;

        _teamsScore[teamIndex].text = value.Score.ToString();

        return;
        Debug.Log("key " + key);
        Debug.Log("value " + value.Score);
        value.OnChange += (changes) => 
        {
            foreach (var change in changes)
            {
                switch (change.Field)
                {
                    case "Score":
                        _teamsScore[value.TeamIndex.ConvertTo<int>()].text = change.Value.ToString();
                        break;
                }
            }
        };
    }

    public void RefreshScore(int teamindex, int score)
    {
        _teamsScore[teamindex].text = score.ToString();
    }

}