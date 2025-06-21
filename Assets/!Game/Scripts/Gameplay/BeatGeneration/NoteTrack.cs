using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(255f / 255f, 249f / 255f, 203 / 255f)]
[TrackBindingType(typeof(TrackContainer))]
[TrackClipType(typeof(NoteClip))]
public class NoteTrack : TrackAsset
{
    [SerializeField, HideInInspector] private TrackContainer _trackBinding;
    [SerializeField, HideInInspector] private PlayableDirector _director;

    public TrackContainer TrackBinding
    {
        get
        {
            if(_director == null)
            {
                _director = GetDirector();
            }

            if (_trackBinding == null && _director != null)
            {
                _trackBinding = _director.GetGenericBinding(this) as TrackContainer;
            }
            return _trackBinding;
        }
    }

    [ExecuteInEditMode]
    public PlayableDirector GetDirector ()
    {
        foreach (var director in Object.FindObjectsOfType<PlayableDirector>())
        {
            if (director.playableAsset is TimelineAsset timelineAsset)
            {
                if (timelineAsset.GetOutputTracks().Contains(this))
                {
                    return director;
                }
            }
        }

        return null;
    }

    public override Playable CreateTrackMixer (PlayableGraph graph, GameObject go, int inputCount)
    {
        _director = GetDirector();
        return base.CreateTrackMixer (graph, go, inputCount);
    }
}

