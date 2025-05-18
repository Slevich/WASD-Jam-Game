using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerInstance : MonoBehaviour
{
    [Header("All input is currently enabled?"), SerializeField, ReadOnly, Space(5)] 
    private bool _inputIsEnabled = false;

    [Header("Player input is currently enabled?"), SerializeField, ReadOnly, Space(5)]
    private bool _playerInputIsEnabled = false;

    private void OnEnable ()
    {
        InputHandler.EnableInput();
        _inputIsEnabled = true;
    }

    private void OnDisable ()
    {
        InputHandler.DisableInput();
        _inputIsEnabled = false;
    }

    public void LockPlayerInput()
    {
        _playerInputIsEnabled = false;
        InputHandler.DisablePlayerInput();
    }

    public void UnlockPlayerInput()
    {
        _playerInputIsEnabled = true;
        InputHandler.EnablePlayerInput();
    }
}
