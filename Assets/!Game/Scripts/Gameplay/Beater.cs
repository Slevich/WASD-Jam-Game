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
    private float _objectToBeatDetectingTimeStep = 0.1f;
    private ActionInterval _detectingInterval;
    private IHitReceiving _hitReceiver;
    private bool _countingLocked = true;
    #endregion

    #region Properties
    public int HitCount { get; set; } = 0;
    public int MissedCount { get; set; } = 0;
    #endregion

    #region Methods
    private void Awake ()
    {
        if (_storage != null)
            _storage.SubscribeOnCollectionChanged(OnCollectionChanged);

        _detectingInterval = new ActionInterval();
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
                Debug.Log("Recivers in range: " + closeObj.Count);
                var orderingCloseObjects = closeObj.OrderByDescending(pair => pair.Value);
                GameObject closeObject = orderingCloseObjects.FirstOrDefault().Key;
                IHitReceiving receiver = (IHitReceiving)(ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(closeObject, typeof(IHitReceiving)));

                if(receiver != null)
                    _hitReceiver = receiver;
            }
            else
            {
                _hitReceiver = null;
            }
        };

        _detectingInterval.StartInterval(_objectToBeatDetectingTimeStep, detectingAction);
    }

    public void StopDetectingObjectsToBeat()
    {
        if(_detectingInterval != null && _detectingInterval.Busy)
            _detectingInterval.Stop();
    }

    public void UnlockCounting() => _countingLocked = false;
    public void LockCounting() => _countingLocked = true;

    public void BeatHit()
    {
        OnBeat?.Invoke();

        if(!_countingLocked)
            MissedCount++;

        if (_hitReceiver == null)
            return;

        if (_hitReceiver.AlreadyHitted)
            return;

        _hitReceiver.HitThis();
        OnHit?.Invoke();

        if(!_countingLocked)
            HitCount++;
    }

    private void OnDisable () => StopDetectingObjectsToBeat();
    #endregion
}
