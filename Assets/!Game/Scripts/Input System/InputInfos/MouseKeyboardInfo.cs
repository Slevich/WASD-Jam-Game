using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseKeyboardInfo : InputInfo, IInputInfo
{
    #region Fields
    private InputActionPhase _fireButtonState = InputActionPhase.Disabled;
    #endregion

    #region Properties
    #endregion

    #region Constructor
    public MouseKeyboardInfo (InputSystem_Actions Actions) : base (Actions)
    {
        
    }
    #endregion

    #region Methods
    public Vector2 ReturnInputDirection () { return Vector2.zero; }
    public InputActionPhase ReturnInputState() { return InputActionPhase.Disabled; }
    public bool FireButtonPerformed () { return false; }
    #endregion
}