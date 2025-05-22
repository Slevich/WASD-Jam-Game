using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferencesContainer : MonoBehaviour
{
    private static PlayerReferencesContainer _instance;
    public static PlayerReferencesContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerReferencesContainer>();
            }

            return _instance;
        }
    }

    [field: Header("Input."), Space(5)]
    [field: SerializeField] public InputHandlerInstance InputState { get; set; }

    [field: Header("Selection"), Space(5)]
    [field: SerializeField] public SelectiveObjectsSequence SelectedConveyor { get; set; }

    [field:Header("Note spawner."), Space(5)]
    [field: SerializeField] public ObjectSpawnManager SpawnManager { get; set; }

    [field:Header("Score manager."), Space(5)]
    [field: SerializeField] public ScoreManager Score { get; set; }

    [field:Header("Gameplay settings."), Space(5)]
    [field: SerializeField] public GameplaySettings Settings { get; set; }

    [field: Header("Gameplay states."), Space(5)]
    [field: SerializeField] public GameStatesController StatesController { get; set; }
}
