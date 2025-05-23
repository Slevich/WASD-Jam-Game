using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    #region Fields
    [field: Header("All notes amount."), SerializeField, ReadOnly] public int AllNotesCount { get; set; } = 0;
    [field: Header("Hitted notes amount."), SerializeField, ReadOnly] public int HittedNotesCount { get; set; } = 0;
    [field: Header("Missed notes amount."), SerializeField, ReadOnly] public int MissedNotesCount { get; set; } = 0;

    [field: Header("Current rank."), SerializeField, ReadOnly] public string Rank { get; set; } = "";

    [SerializeField] private UnityEvent<float> _onRankChanged;

    private float _rankScore = 0;

    private float _hitRankIncrement = 0.01f;
    private float _hitRankDicrement = 0.05f;
    #endregion

    #region Methods
    public void IncrementAllNotesCount ()
    {
        AllNotesCount++;
    }

    public void IncrementHittedNotesCount ()
    {
        HittedNotesCount++;

        float currentRankScore = _rankScore + _hitRankIncrement;
        _rankScore = Mathf.Clamp(currentRankScore, -1, 1);
        _onRankChanged?.Invoke(_rankScore);
    }


    public void IncrementMissedNotesCount ()
    {
        MissedNotesCount++;

        float currentRankScore = _rankScore - _hitRankDicrement;
        _rankScore = Mathf.Clamp(currentRankScore, -1, 1);
        _onRankChanged?.Invoke(_rankScore);
    }

    public void CheckMissedNoteAndIncrement(GameObject DestroyedObject)
    {
        IHitReceiving hitReceiving = (IHitReceiving)(ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(DestroyedObject, typeof(IHitReceiving)));

        if (hitReceiving != null)
        {
            bool isMissed = !hitReceiving.AlreadyHitted;

            if (isMissed)
            {
                IncrementMissedNotesCount();
            }
        }
    }
    #endregion
}


public enum Ranks
{
    Worker,
    Beater,
    ShiftManager,
    Slacker,
    Digrace
}