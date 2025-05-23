using UnityEditor;
using UnityEngine;

public class GameplaySettings : MonoBehaviour
{
    #region Fields
    [Header("Speed modifier of the notes movement."), SerializeField, Range(0, 10f)] private float _notesSpeed = 1f;
    [Header("Timestep in seconds between notes."), SerializeField, Range(0, 10f)] private float _notesTimeStep = 1f;

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
    public float NotesTimeStep => _notesTimeStep;
    #endregion

    #region Methods
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameplaySettings))]
public class GameplaySettingsEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI();
        SerializedProperty timeStepProperty = serializedObject.FindProperty("_notesTimeStep");
        float timestepValue = timeStepProperty.floatValue;
        StaticValues.ClipsMinDistanceTime = timestepValue;
    }
}
#endif