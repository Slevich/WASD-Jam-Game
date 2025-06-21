using UnityEditor;
using UnityEngine;

public class GameplaySettings : MonoBehaviour
{
    #region Fields
    [Header("Speed modifier of the notes movement."), SerializeField, Range(0, 10f)] private float _notesSpeed = 1f;
    private static GameplaySettings _instance;
    #endregion

    #region Properties
    public static GameplaySettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameplaySettings>();
            }

            return _instance;
        }
    }

    public float NotesSpeed => _notesSpeed;
    #endregion

    #region Methods
    #endregion
}