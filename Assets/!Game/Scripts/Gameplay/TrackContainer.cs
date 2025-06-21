using System;
using UnityEngine;
using System.Collections.Generic;

public class TrackContainer : MonoBehaviour
{
    [field: Header("Spawner component."), SerializeField] public ObjectSpawner Spawner { get; set; }
    [field: Header("Box movement manager (animation)."), SerializeField] public ConveyorMovementAnimation MovementManager { get; set; }
    [field: Header("Selective object."), SerializeField] public SelectiveObject SelectiveObject { get; set; }
}