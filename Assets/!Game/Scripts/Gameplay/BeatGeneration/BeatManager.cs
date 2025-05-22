using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BeatManager : MonoBehaviour
{
    #region Fields
    [Header("Playable director with beat track."), SerializeField] private PlayableDirector _timeline;

    private float _beatUpdateTime = 0.1f;
    private ActionInterval _beatInteval;
    private ObjectSpawnManager _spawnManager;
    private int _lastLineSpawnIndex = -1;

    private TimelineAsset _timelineAsset;
    private List<NoteSpawnMoment> _notesMoments = new List<NoteSpawnMoment>();
    private Queue<NoteSpawnMoment> _notesQueue = new Queue<NoteSpawnMoment>();
    #endregion

    #region Methods
    private void Awake ()
    {
        _beatInteval = new ActionInterval();
        _spawnManager = PlayerReferencesContainer.Instance.SpawnManager;

        if (_timeline != null)
        {
            _timelineAsset = (TimelineAsset)_timeline.playableAsset;

            if (_timelineAsset != null)
            {
                TrackAsset[] tracks = _timelineAsset.GetOutputTracks().ToArray();

                NoteTrack[] noteTracks = tracks.Where(track => track is NoteTrack).Select(track => track as NoteTrack).ToArray();

                foreach (NoteTrack track in noteTracks)
                {
                    IEnumerable<TimelineClip> clips = track.GetClips();

                    if(clips == null || clips.Count() == 0)
                        continue;

                    TimelineClip[] trackClips = track.GetClips().ToArray();
                    UnityEngine.Object[] clipsObjects = trackClips.Select(clip => clip.asset).ToArray();
                    NoteClip[] noteClips = clipsObjects.Select(clipObj => clipObj as NoteClip).ToArray();

                    if(noteClips != null && noteClips.Length > 0)
                    {
                        foreach(NoteClip clip in noteClips)
                        {
                            NoteSpawnMoment moment = new NoteSpawnMoment(clip.Spawner, clip.TimePosition);
                            _notesMoments.Add(moment);
                        }
                    }
                }
            }

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

    private void OnDisable () => StopNoteSpawn();
    #endregion

    private class NoteSpawnMoment
    {
        public ObjectSpawner Track { get; private set; }
        public float Time { get; private set; }

        public NoteSpawnMoment(ObjectSpawner Container, float TimeDuration)
        {
            Track = Container;
            Time = TimeDuration;
        }
    }
}
