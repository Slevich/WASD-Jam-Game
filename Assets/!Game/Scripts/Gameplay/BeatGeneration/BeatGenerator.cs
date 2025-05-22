using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class BeatGenerator
{
    private AudioClip _beatClip;
    private float[] _samples;
    public float _volumeThreshold = 0.1f;
    public float _timeBetweenNotes = 0.2f;
    List<float> noteTimes = new List<float>();

    public float[] Samples => _samples;

    public BeatGenerator (AudioClip BeatClip, float TimeBetweenNotes, float VolumeThreshold)
    {
        _beatClip = BeatClip;
        _timeBetweenNotes = TimeBetweenNotes;
        _volumeThreshold = VolumeThreshold;

        if (_beatClip != null)
            _samples = new float[_beatClip.samples * _beatClip.channels];
    }

    public List<float> GenerateBeat ()
    {
        if (_samples == null)
            return new List<float>();
        
        _beatClip.GetData(_samples, 0);
        AnalyzeAndGenerateNotes(_samples);
        Debug.Log("Notes generated!");
        return noteTimes;
    }

    void AnalyzeAndGenerateNotes (float[] samples)
    {
        int samplesPerSecond = _beatClip.frequency;
        float timeElapsed = 0f;
        int samplesStep = samplesPerSecond / 10;

        for (int i = 0; i < samples.Length; i += samplesStep)
        {
            float volume = Mathf.Abs(samples[i]);

            if (volume > _volumeThreshold && i > 0)
            {
                noteTimes.Add(timeElapsed);
            }

            timeElapsed += 0.1f;
        }
    }
}
