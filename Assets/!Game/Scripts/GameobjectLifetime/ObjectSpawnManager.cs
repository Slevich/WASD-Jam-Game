using System;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawnManager : MonoBehaviour
{
    #region Fields
    [Header("Spawners to manage."), SerializeField] private ObjectSpawner[] _spawners;
    [Header("Event on each spawn!"), SerializeField] private UnityEvent _onEachSpawn;
    [Header("Event on each spawned object destroy!"), SerializeField] private UnityEvent<GameObject> _onEachObjectDestroy;

    //private float _beatUpdateTime = 1f;
    //private ActionInterval _beatInteval;
    #endregion

    #region Methods
    private void Awake ()
    {
        //_beatInteval = new ActionInterval();
        //_beatUpdateTime = GameplaySettings.Instance.NotesTimeStep;
    }

    //public void StartSpawn()
    //{
    //    if (_beatInteval != null && _beatInteval.Busy)
    //        return;

    //    Action spawnAction = delegate
    //    {
    //        foreach(ObjectSpawner spawner in _spawners)
    //        {
    //            SingleSpawn(spawner);
    //        }
    //    };

    //    _beatInteval.StartInterval(_beatUpdateTime, spawnAction);
    //}

    public void SpawnOnSpawnerByID(int Index)
    {
        if(Index < 0 || Index >= _spawners.Length)
        {
            Debug.LogError("Index of spawner is out of range!");
            return;
        }

        ObjectSpawner spawner = _spawners[Index];
        SingleSpawn(spawner);
    }

    private void SingleSpawn(ObjectSpawner spawner)
    {
        GameObject spawnedObject = spawner.SpawnNewObject();
        _onEachSpawn?.Invoke();

        OnObjectDestroy onDestroy = null;
        onDestroy = (OnObjectDestroy)(ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(spawnedObject, typeof(OnObjectDestroy)));

        if (onDestroy == null)
            onDestroy = (OnObjectDestroy)(spawnedObject.AddComponent(typeof(OnObjectDestroy)));

        onDestroy.OnDestroyCallback += delegate { _onEachObjectDestroy?.Invoke(spawnedObject); };
    }

    //public void StopSpawn()
    //{
    //    if(_beatInteval != null && _beatInteval.Busy)
    //        _beatInteval.Stop();
    //}

    //private void OnDisable () => StopSpawn();
    #endregion
}
