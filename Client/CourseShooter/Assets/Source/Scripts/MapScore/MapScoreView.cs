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
        _presenter.Enable();
    }

    public void OnScoreTeamAdd(string key, MapScoreData value)
    {
        value.OnChange += (changes) => 
        {
            foreach (var change in changes)
            {
                switch (change.Field)
                {
                    case "Score":
                        _presenter.SetScore(value.TeamIndex.ConvertTo<int>(), change.Value.ConvertTo<int>());
                        break;
                }
            }
        };
    }

    public int GetTeamScore(int teamIndex) =>
        _presenter.GetTeamScore(teamIndex);

    public void AddScore(int teamindex)
    {
        _presenter.AddScore(teamindex);
    }

    public void RefreshScore(int teamindex, int score)
    {
        _teamsScore[teamindex].text = score.ToString();
    }
}
