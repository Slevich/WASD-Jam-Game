using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawner : MonoBehaviour
{
    #region Fields
    [Header("Spawned object prefab."), SerializeField] private GameObject _originalObject;
    [Header("Transform position for the spawn."), SerializeField] private Transform _spawnPoint;
    [Header("Transform parent for the spawned object."), SerializeField] private Transform _objectParent;
    [field: Header("Event on spawn new object."), Space(10), SerializeField] public UnityEvent<GameObject> OnSpawnNewObject { get; set; }
    #endregion

    #region Methods
    public GameObject SpawnNewObject()
    {
        if(_originalObject == null)
        {
            Debug.LogError("Object spawner has no reference on prefab!");
            return null;
        }

        Vector3 spawnPosition = _spawnPoint != null ? _spawnPoint.position : transform.position;
        Quaternion spawnRotation = _spawnPoint != null ? _spawnPoint.rotation : Quaternion.identity;

        GameObject spawnedObject = Instantiate(_originalObject, spawnPosition, spawnRotation, _objectParent);
        OnSpawnNewObject?.Invoke(spawnedObject);
        return spawnedObject;
    }
    #endregion
}
