using TMPro;
using UnityEngine;

public class UIStatisticUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _allNotesText;
    [SerializeField] private TextMeshProUGUI _hittedNotesText;
    [SerializeField] private TextMeshProUGUI _missedNotesText;
    [SerializeField] private TextMeshProUGUI _rankText;

    private ScoreManager _scoreManager;

    private void Awake ()
    {
        _scoreManager = PlayerReferencesContainer.Instance.Score;
    }

    public void UpdateResult()
    {
        if (_scoreManager == null)
            return;

        if(_allNotesText != null)
            _allNotesText.text = _scoreManager.AllNotesCount.ToString();

        if (_hittedNotesText != null)
            _hittedNotesText.text = _scoreManager.HittedNotesCount.ToString();

        if (_missedNotesText != null)
            _missedNotesText.text = _scoreManager.MissedNotesCount.ToString();

        if (_rankText != null)
            _rankText.text = _scoreManager.Rank.ToString();
    }
}
