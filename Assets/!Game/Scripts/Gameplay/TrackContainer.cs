using System;
using UnityEngine;

public class TrackContainer : MonoBehaviour
{
    #region Fields
    [field: Header("Spawner info."), SerializeField] public TrackInfo Info { get; set; }
    #endregion
}

[Serializable]
public class TrackInfo
{
    [field: Header("Selectable component."), SerializeField] public SelectiveObject Selective { get; set; }
    [field: Header("Spawner component."), SerializeField] public ObjectSpawner Spawner { get; set; }
}
