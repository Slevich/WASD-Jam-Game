using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandlerStateDisplay : MonoBehaviour
{
    [Header("Input is currently enabled?"), SerializeField, ReadOnly, Space(5)] 
    private bool _inputIsEnabled = false;

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
}
