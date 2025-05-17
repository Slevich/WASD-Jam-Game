using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputInfo
{
    public InputActionPhase UpButtonPhase { get; }
    public InputActionPhase DownButtonPhase { get; }
    public InputActionPhase HitButtonPhase { get; }
}
