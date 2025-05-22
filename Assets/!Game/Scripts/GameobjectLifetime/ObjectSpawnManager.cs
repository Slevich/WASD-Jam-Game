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

    private void Start ()
    {
        if (_spawners == null || _spawners.Length == 0)
            return;

        foreach(ObjectSpawner spawner in _spawners)
        {
            spawner.OnSpawnNewObject.AddListener(ManageSpawnedObject);
        }
    }

    private void ManageSpawnedObject(GameObject spawnedObject)
    {
        _onEachSpawn?.Invoke();

        OnObjectDestroy onDestroy = null;
        onDestroy = (OnObjectDestroy)(ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(spawnedObject, typeof(OnObjectDestroy)));

        if (onDestroy == null)
            onDestroy = (OnObjectDestroy)(spawnedObject.AddComponent(typeof(OnObjectDestroy)));

        onDestroy.OnDestroyCallback += delegate { _onEachObjectDestroy?.Invoke(spawnedObject); };
    }
    #endregion
}
