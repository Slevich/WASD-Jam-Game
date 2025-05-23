using UnityEngine;

[CreateAssetMenu(fileName = "BeatContainer", menuName = "Scriptable Objects/Beat container")]
public class BeatContainerScriptable : ScriptableObject
{
    [field: SerializeField, HideInInspector] public NoteSpawnMoment[] SpawnMoment { get; set; }
}
