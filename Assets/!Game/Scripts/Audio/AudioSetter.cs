using UnityEngine;

public class AudioSetter : MonoBehaviour
{
    #region Fields
    [Header("Audio source to play."), SerializeField] private AudioSource _source;
    #endregion

    #region Methods
    public void SetClipAndPlay(AudioClip Clip)
    {
        if (_source == null)
            return;

        _source.clip = Clip;
        _source.Play();
    }

    public void SetClipStopPreviousAndPlay (AudioClip Clip)
    {
        if (_source == null)
            return;

        if(_source.isPlaying)
        {
            _source.Stop();
        }

        SetClipAndPlay(Clip);
    }

    public void SetClip(AudioClip Clip)
    {
        if (_source == null)
            return;

        _source.clip = Clip;
    }
    #endregion
}
