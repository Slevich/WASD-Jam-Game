using System;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BeatManager : MonoBehaviour
{
    #region Fields
    [Header("Audio source to play music."), SerializeField] private AudioSource _audioSource;
    [Header("Tracks infos."), SerializeField] private TrackContainer[] _tracks;

    private float _beatUpdateTime = 0.01f;
    private NotesGenerator _notesGenerator;
    private ActionInterval _beatInteval;
    private NoteSpawnMoment[] _notesMoments = new NoteSpawnMoment[] { };
    private Queue<NoteSpawnMoment> _notesQueue = new Queue<NoteSpawnMoment>();
    #endregion

    #region Methods
    private void Awake ()
    {
        _beatInteval = new ActionInterval();
        _notesGenerator = new NotesGenerator(_tracks.Length);
        _notesMoments = _notesGenerator.GetNotesSpawnMoments();
        _notesMoments = _notesMoments.OrderBy(moment => moment.Time).ToArray();
        
        if (_notesMoments.Length > 0)
        {
            for (int i = 0; i < _notesMoments.Length; i++)
            {
                _notesQueue.Enqueue(_notesMoments[i]);
            }
        }

        _notesMoments = new NoteSpawnMoment[0];
    }

    public void StartNoteSpawn()
    {
        if(_audioSource == null || _audioSource.clip == null)
        {
            Debug.LogError("Can't play a song!");
            return;
        }

        if (_beatInteval != null && _beatInteval.Busy)
            return;

        float spawnTime = 0f;
        _audioSource.Play();

        Action beatAction = delegate
        {
            spawnTime += _beatUpdateTime;

            if (_notesQueue.Count == 0)
            {
                StopNoteSpawn();
                return;
            }

            NoteSpawnMoment moment = _notesQueue.Peek();
            TrackContainer spawnTrack = _tracks[moment.TrackIndex];
            float noteSpawnTime = moment.Time - spawnTrack.MovementManager.TotalMovementDuration;
            float audioTime = _audioSource.time;
            Debug.Log(audioTime);

            if (audioTime >= noteSpawnTime)
            {
                moment = _notesQueue.Dequeue();
                spawnTrack.Spawner.SpawnNewObject();
            }
        };

        _beatInteval.StartInterval(_beatUpdateTime, beatAction);
    }

    public void StopNoteSpawn()
    {
        if(_audioSource != null && _audioSource.isPlaying)
            _audioSource.Stop();

        if(_beatInteval != null && _beatInteval.Busy)
            _beatInteval.Stop();
    }

    private void OnDisable () => StopNoteSpawn();
    #endregion

}

[Serializable]
public class NoteSpawnMoment
{
    [field: SerializeField] public int TrackIndex { get; private set; } = 0;
    [field: SerializeField] public float Time { get; private set; } = 0f;

    public NoteSpawnMoment (int Index, float TimeDuration)
    {
        TrackIndex = Index;
        Time = TimeDuration;
    }
}

public class NotesGenerator
{
    public static string MidiFilePath => Application.streamingAssetsPath;
    public static string MidiFileName => "Bit.mid";
    private MidiFile _midiFile;
    private int _tracksCount = 0;

    public NotesGenerator (int TracksCount)
    {
        if(TracksCount <= 0)
        {
            _tracksCount = 0;
            return;
        }

        _tracksCount = TracksCount;
    }

    public NoteSpawnMoment[] GetNotesSpawnMoments ()
    {
        if (_tracksCount == 0)
            return new NoteSpawnMoment[0];

        ReadFromMidiFile();
        Melanchall.DryWetMidi.Interaction.Note[] notes = GetDataFromMidi();

        if (notes.Length == 0)
            return new NoteSpawnMoment[0];

        NoteName[] notesNames = notes.Select(note => note.NoteName).ToArray();
        List<NoteName> uniqieNames = new List<NoteName>();

        foreach (NoteName noteName in notesNames)
        {
            if(!uniqieNames.Contains(noteName))
                uniqieNames.Add(noteName);
        }

        uniqieNames = uniqieNames.OrderBy(name => (int)name).ToList();

        List<NoteSpawnMoment> spawnMoments = new List<NoteSpawnMoment>();

        foreach(var note in notes)
        {
            MetricTimeSpan metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _midiFile.GetTempoMap());
            double span = (double)metricTimeSpan.Minutes * 60 + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f;
            int trackIndex = uniqieNames.IndexOf(note.NoteName);
            NoteSpawnMoment spawnMoment = new NoteSpawnMoment(trackIndex, (float)span);
            spawnMoments.Add(spawnMoment);
        }

        return spawnMoments.ToArray();
    }

    private void ReadFromMidiFile ()
    {
        _midiFile = MidiFile.Read(MidiFilePath + "/" + MidiFileName);
    }

    private Melanchall.DryWetMidi.Interaction.Note[] GetDataFromMidi()
    {
        if(_midiFile == null)
        {
            Debug.LogError("Midi file is null!");
            return new Melanchall.DryWetMidi.Interaction.Note[0];
        }

        ICollection<Melanchall.DryWetMidi.Interaction.Note> notes = _midiFile.GetNotes();
        Melanchall.DryWetMidi.Interaction.Note[] notesArray = notes.ToArray();
        return notesArray;
    }
}