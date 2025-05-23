using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerInstance : MonoBehaviour
{
    [Header("All input is currently enabled?"), SerializeField, ReadOnly, Space(5)] 
    private bool _inputIsEnabled = false;

    [Header("Player input is currently enabled?"), SerializeField, ReadOnly, Space(5)]
    private bool _playerInputIsEnabled = false;

    [Header("UI input is currently enabled?"), SerializeField, ReadOnly, Space(5)]
    private bool _uIInputIsEnabled = false;

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

    public void LockUIInput()
    {
        _uIInputIsEnabled = false;
        InputHandler.DisableUIInput();
    }

    public void UnlockUIInput ()
    {
        _uIInputIsEnabled = true;
        InputHandler.EnableUIInput();
    }
}
