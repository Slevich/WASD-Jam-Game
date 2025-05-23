using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;

    private float _currentRankValue = 0;
    private float _minRankValue = -1;
    private float _maxRankValue = 1;
    private Ranks[] _ranks;
    private Ranks _currentRank;
    private Dictionary<Ranks, string> _ranksDictionary = new Dictionary<Ranks, string>();
    private int _ranksCount = 0;

    private void Awake ()
    {
        _ranks = new Ranks[]    
        { 
            Ranks.ShiftManager,
            Ranks.Beater,
            Ranks.Worker,
            Ranks.Slacker,
            Ranks.Digrace
        };

        _ranksDictionary = new Dictionary<Ranks, string>
        {
            { Ranks.ShiftManager, "Начальник смены"},
            { Ranks.Beater, "Ударник"},
            { Ranks.Worker, "Работяга" },
            { Ranks.Slacker, "Лентяй"},
            { Ranks.Digrace, "Позор коллектива"}
        };

        _ranksCount = _ranks.Length;
        UpdateRank(0);
    }

    public void UpdateRank(float RankValue)
    {
        if (_rankText == null)
            return;

        if (RankValue < (_minRankValue - (_minRankValue / 4)))
            _currentRank = Ranks.Digrace;
        else if (RankValue < (_minRankValue - ((_minRankValue / 4) * 3)))
            _currentRank = Ranks.Slacker;
        else if (RankValue >= (_minRankValue - ((_minRankValue / 4) * 3)) && RankValue <= (_maxRankValue - ((_maxRankValue / 4) * 3)))
            _currentRank = Ranks.Worker;
        else if (RankValue >= (_minRankValue - ((_minRankValue / 4) * 3)))
            _currentRank = Ranks.Beater;
        else if (RankValue >= (_minRankValue - ((_minRankValue / 4))))
            _currentRank = Ranks.ShiftManager;

        _rankText.text = _ranksDictionary[_currentRank];
    }
}
