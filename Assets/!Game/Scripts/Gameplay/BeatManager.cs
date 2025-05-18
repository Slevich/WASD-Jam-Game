using System;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    #region Fields
    private float _beatUpdateTime = 1f;
    private ActionInterval _beatInteval;
    #endregion

    #region Methods
    private void Awake ()
    {
        _beatInteval = new ActionInterval();
        _beatUpdateTime = GameplaySettings.Instance.NotesTimeStep;
    }

    public void StartBeatGeneration()
    {
        if (_beatInteval != null && _beatInteval.Busy)
            return;

        Action beatAction = delegate
        {

        };

        _beatInteval.StartInterval(_beatUpdateTime, beatAction);
    }

    public void StopBeatGeneration()
    {
        if(_beatInteval != null && _beatInteval.Busy)
            _beatInteval.Stop();
    }

    private void OnDisable () => StopBeatGeneration();
    #endregion
}
