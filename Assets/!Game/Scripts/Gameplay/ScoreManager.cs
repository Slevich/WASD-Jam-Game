using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Fields
    [field: Header("All notes amount."), SerializeField, ReadOnly] public int AllNotesCount { get; set; } = 0;
    [field: Header("Hitted notes amount."), SerializeField, ReadOnly] public int HittedNotesCount = 0;
    [field: Header("Missed notes amount."), SerializeField, ReadOnly] public int MissedNotesCount = 0;
    #endregion

    #region Methods
    public void IncrementAllNotesCount() => AllNotesCount++;
    public void IncrementHittedNotesCount() => HittedNotesCount++;
    public void IncrementMissedNotesCount() => MissedNotesCount++;

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
