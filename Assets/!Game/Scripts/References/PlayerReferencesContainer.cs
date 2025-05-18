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

    [Header("Input."), Space(5)]
    [SerializeField] private InputHandlerInstance InputState;

    [Header("Selection"), Space(5)]
    [SerializeField] private SelectiveObjectsSequence SelectedConveyor;

    [Header("Score manager."), Space(5)]
    [SerializeField] private ScoreManager Score;

    [Header("Gameplay settings."), Space(5)]
    [SerializeField] private GameplaySettings Settings;
}
