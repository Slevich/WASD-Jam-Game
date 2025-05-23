using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class NoteClip : PlayableAsset
{
    [SerializeField, TypeMismatchFix] private ObjectSpawner _spawner;
    [SerializeField] private float _timePosition;

    public ObjectSpawner Spawner { get { return _spawner; } set { _spawner = value; } }
    public float TimePosition { get { return _timePosition; } set { _timePosition = value; } }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<NoteBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.NotePlayable = this;

        var timelineClip = this.GetParentClip(); // см. ниже реализацию GetParentClip
        if (timelineClip != null)
        {
            timelineClip.duration = 0.2;
            behaviour.ClipStart = timelineClip.start;
            behaviour.ClipDuration = timelineClip.duration;
        }

        return playable;
    } 
}

public static class PlayableAssetExtensions
{
    public static TimelineClip GetParentClip (this PlayableAsset asset)
    {
        // Обходной путь: поиск по всем клипам в активном Timeline Asset
        foreach (var view in TimelineEditorUtilities.GetAllTimelineAssets())
        {
            foreach (var track in view.GetOutputTracks())
            {
                foreach (var clip in track.GetClips())
                {
                    if (clip.asset == asset)
                        return clip;
                }
            }
        }

        return null;
    }
}

// Вспомогательный класс:
public static class TimelineEditorUtilities
{
    public static IEnumerable<TimelineAsset> GetAllTimelineAssets ()
    {
        List<TimelineAsset> timelines = new List<TimelineAsset>();

        string[] guids = AssetDatabase.FindAssets("t:TimelineAsset");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            timelines.Add(AssetDatabase.LoadAssetAtPath<TimelineAsset>(path));
        }

        return timelines;
    }
}