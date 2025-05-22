using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameStatesController : MonoBehaviour
{
    #region Fields
    [Header("Game states."), SerializeField] private GameState[] _states;

    private GameState _activeState;
    #endregion

    #region Methods
    public void ChangeState(string StateName)
    {
        IEnumerable<GameState> statesWithName = _states.Where(state => state.Name == StateName);

        if (statesWithName == null)
            return;

        GameState state = statesWithName.FirstOrDefault();

        if (_activeState == state)
            return;

        if (_activeState != null)
        {
            _activeState.Active = false;
            _activeState.OnDisactiveState?.Invoke();
        }

        _activeState = state;
        _activeState.Active = true;
        _activeState?.OnActiveState?.Invoke();
    }

    public void ChangeState (GameStates NewState)
    {
        string name = NewState.ToString();
        ChangeState(name);
    }
    #endregion
}

[Serializable]
public class GameState
{
    [field: SerializeField] public GameStates State { get; set; }
    [field: SerializeField] public UnityEvent OnActiveState { get; set; }
    [field: SerializeField] public UnityEvent OnDisactiveState { get; set; }
    [field: SerializeField, ReadOnly] public bool Active { get; set; }

    public string Name => State.ToString();
}

public enum GameStates
{
    Introduction,
    Play,
    Pause,
    End,
    Exit
}
