using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class Beater : MonoBehaviour
{
    #region Fields
    [Header("Object storage."), SerializeField] private ObjectsStorage _storage;
    [Header("Sphere Area of the beat."), SerializeField] private CircleArea _beatArea;
    [field: Header("Event on beat."), SerializeField] public UnityEvent OnBeat { get; set; }
    [field: HideInInspector] public UnityEvent OnHit { get; set; } = new UnityEvent();


    private List<GameObject> _objectsToBeat = new List<GameObject>();
    private float _objectToBeatDetectingTimeStep = 0.05f;
    private ActionInterval _detectingInterval;
    private ActionTimer _stopDetectingTimer;
    private float _stopDetectingDelay = 0.2f;
    private IHitReceiving _hitReceiver;
    #endregion

    #region Methods
    private void Awake ()
    {
        if (_storage != null)
            _storage.SubscribeOnCollectionChanged(OnCollectionChanged);

        _detectingInterval = new ActionInterval();
        _stopDetectingTimer = new ActionTimer();
    }

    public void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        switch(args.Action)
        {
            case NotifyCollectionChangedAction.Add:
                _objectsToBeat.Add((GameObject)args.NewItems?[0]);
                break;

            case NotifyCollectionChangedAction.Remove:
                _objectsToBeat.RemoveAll(obj => obj == null);
                break;

            default:
                break;
        }
    }

    public void StartDetectingObjectsToBeat()
    {
        if (_stopDetectingTimer != null && _stopDetectingTimer.Busy)
            _stopDetectingTimer.StopTimer();
        
        if(_detectingInterval != null && _detectingInterval.Busy)
            return;
            
        Action detectingAction = delegate
        {
            Dictionary<GameObject, float> closeObj = new Dictionary<GameObject, float>();

            foreach (GameObject obj in _objectsToBeat)
            {
                if (obj == null)
                    continue;

                bool isInArea = _beatArea.PointIsInArea(obj.transform.position, out float Approaching);

                if(isInArea)
                {
                    closeObj.Add(obj, Approaching);
                }
            }

            if(closeObj.Count > 0)
            {
                var orderingCloseObjects = closeObj.OrderByDescending(pair => pair.Value);
                IHitReceiving receiver = null;

                foreach (var obj in orderingCloseObjects)
                {
                    GameObject receiverObj = obj.Key;
                    receiver = (IHitReceiving)(ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(receiverObj, typeof(IHitReceiving)));

                    if (receiver != null && receiver.AlreadyHitted == false)
                    {
                        _hitReceiver = receiver;
                        break;
                    }
                    else
                        _hitReceiver = null;
                }
            }
            else
            {
                _hitReceiver = null;
            }

            if (_hitReceiver != null)
            {
                BeatHit();
            }
        };

        _detectingInterval.StartInterval(_objectToBeatDetectingTimeStep, detectingAction);
    }

    public void StopDetectingObjectsToBeat()
    {
        if(_stopDetectingTimer != null && _stopDetectingTimer.Busy)
            return;
        
        Action stopDetectingAction = delegate
        {
            if(_detectingInterval != null && _detectingInterval.Busy)
                _detectingInterval.Stop();
        };
        
        _stopDetectingTimer.StartTimerAndAction(_stopDetectingDelay, stopDetectingAction);
    }

    public void BeatHit()
    {
        OnBeat?.Invoke();
        _hitReceiver.HitThis();
        OnHit?.Invoke();
    }

    private void OnDisable () => StopDetectingObjectsToBeat();
    #endregion
}
