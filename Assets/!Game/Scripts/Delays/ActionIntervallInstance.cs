using System;
using UnityEngine;
using UnityEngine.Events;

public class ActionIntervallInstance : MonoBehaviour
{
    #region Fields
    [Header("Event calling on interval."), SerializeField] private UnityEvent _onInterval;
    [Header("Interval time step in seconds."), SerializeField, Range(0f, 10f)] private float _intervalTimeStep = 1.0f;

    private ActionInterval _interval;
    private Action _intervalAction;
    #endregion

    #region Methods
    private void Awake ()
    {
        _interval = new ActionInterval();
        _intervalAction = delegate { _onInterval?.Invoke(); };
    }

    public void StartInterval()
    {
        if (_interval != null && _interval.Busy)
            return;

        _interval.StartInterval(_intervalTimeStep, _intervalAction);
    }

    public void StopInterval ()
    {
        if (_interval != null && _interval.Busy)
            _interval.Stop();
    }

    private void OnDisable () => StopInterval();
    #endregion
}
