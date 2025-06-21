using System;
using System.IO.IsolatedStorage;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class NoteBehaviour : PlayableBehaviour
{
    public NoteClip NotePlayable { get; set; }

    public double ClipStart { get; set; } = 0f;
    public double ClipDuration { get; set; } = 0f;

    //private bool _noteSpawned = false;

    public override void ProcessFrame (Playable playable, FrameData info, object playerData)
    {
        TrackContainer clipTrack = playerData as TrackContainer;

        if (clipTrack == null || NotePlayable == null)
            return;

        float currentTime = (float)(playable.GetTime());
        //NotePlayable.TrackIndex = clipTrack.Info.TrackIndex;
        //NotePlayable.TimePosition = (float)(ClipStart + (ClipDuration / 2));

        //if(Application.IsPlaying(NotePlayable.Spawner) && !_noteSpawned)
        //{
        //    NotePlayable.Spawner.SpawnNewObject();
        //    _noteSpawned = true;
        //}
    }
}