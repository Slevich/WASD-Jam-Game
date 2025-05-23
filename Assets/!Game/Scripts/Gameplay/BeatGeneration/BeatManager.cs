using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BeatManager : MonoBehaviour
{
    #region Fields
    [Header("Playable director with beat track."), SerializeField] private PlayableDirector _timeline;
    [Header("Scriptable to save beat."), SerializeField] private BeatContainerScriptable _beatContainer;

    private float _beatUpdateTime = 0.1f;
    private ActionInterval _beatInteval;
    private ObjectSpawnManager _spawnManager;
    private int _lastLineSpawnIndex = -1;

    private TimelineAsset _timelineAsset;
    private List<NoteSpawnMoment> _notesMoments = new List<NoteSpawnMoment>();
    private Queue<NoteSpawnMoment> _notesQueue = new Queue<NoteSpawnMoment>();
    private static readonly string _jsonName = "beats.json";
    #endregion

    #region Methods
    private void Awake ()
    {
        _beatInteval = new ActionInterval();
        _spawnManager = PlayerReferencesContainer.Instance.SpawnManager;

        if (_timeline != null && _beatContainer != null)
        {
            _notesMoments = _beatContainer.SpawnMoment.ToList();

            if(_notesMoments.Count > 0)
            {
                _notesMoments = _notesMoments.OrderBy(note => note.Time).ToList();
                
                foreach(NoteSpawnMoment moment in _notesMoments)
                {
                    _notesQueue.Enqueue(moment);
                }
            }
        }
    }

    public void StartNoteSpawn()
    {
        if (_beatInteval != null && _beatInteval.Busy)
            return;

        float spawnTime = 0f;

        Action beatAction = delegate
        {
            spawnTime += _beatUpdateTime;

            if(_notesQueue.Count == 0)
            {
                StopNoteSpawn();
                return;
            }

            NoteSpawnMoment moment = _notesQueue.Peek();

            if(moment.Time <= spawnTime)
            {
                moment = _notesQueue.Dequeue();
                //_spawnManager.SpawnOnSpawner(moment.Track);
                _notesMoments.Remove(moment);
            }

            Debug.Log("TIme: " + spawnTime);
        };

        _beatInteval.StartInterval(_beatUpdateTime, beatAction);
    }

    public void StopNoteSpawn()
    {
        if(_beatInteval != null && _beatInteval.Busy)
            _beatInteval.Stop();
    }

    [ExecuteInEditMode]
    public void SaveBeats()
    {
        if(_beatContainer != null && _timeline != null)
        {
            TimelineAsset asset = (TimelineAsset)_timeline.playableAsset;
            IEnumerable<TrackAsset> trackss = asset.GetOutputTracks();

            if (trackss == null || trackss.Count() == 0)
                return;

            TrackAsset[] tracks = trackss.ToArray();
            NoteTrack[] noteTracks = tracks.Where(track => track is NoteTrack).Select(track => track as NoteTrack).ToArray();
            List<NoteSpawnMoment> moments = new List<NoteSpawnMoment>();

            foreach (NoteTrack track in noteTracks)
            {
                IEnumerable<TimelineClip> clips = track.GetClips();

                if (clips == null || clips.Count() == 0)
                    continue;

                TimelineClip[] trackClips = track.GetClips().ToArray();
                UnityEngine.Object[] clipsObjects = trackClips.Select(clip => clip.asset).ToArray();
                NoteClip[] noteClips = clipsObjects.Select(clipObj => clipObj as NoteClip).ToArray();

                if (noteClips != null && noteClips.Length > 0)
                {
                    foreach (NoteClip clip in noteClips)
                    {
                        NoteSpawnMoment moment = new NoteSpawnMoment(clip.Spawner, clip.TimePosition);
                        moments.Add(moment);
                    }
                }
            }

            if(moments.Count > 0)
            {
                _beatContainer.SpawnMoment = moments.ToArray();
                Debug.Log("записал!");
            }

        }
    }

    private void OnDisable () => StopNoteSpawn();
    #endregion

}

[Serializable]
public class NoteSpawnMoment
{
    public ObjectSpawner Track { get; private set; }
    public float Time { get; private set; }

    public NoteSpawnMoment (ObjectSpawner Container, float TimeDuration)
    {
        Track = Container;
        Time = TimeDuration;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(BeatManager))]
public class BeatManagerEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        bool buttonPressed = GUILayout.Button("Save to scriptable!");
        if(buttonPressed)
        {
            BeatManager manager = (BeatManager)target;
            manager.SaveBeats();
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
