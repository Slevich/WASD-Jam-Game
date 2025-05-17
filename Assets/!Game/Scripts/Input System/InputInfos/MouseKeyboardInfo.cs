using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseKeyboardInfo : InputInfo, IInputInfo
{
    #region Fields
    private InputSystem_Actions _inputActions;
    #endregion

    #region Properties
    public InputActionPhase UpButtonPhase => _inputActions.PlayerActions.Up.phase;
    public InputActionPhase DownButtonPhase => _inputActions.PlayerActions.Down.phase;
    public InputActionPhase HitButtonPhase => _inputActions.PlayerActions.Hit.phase;
    #endregion

    #region Constructor
    public MouseKeyboardInfo (InputSystem_Actions Actions) : base (Actions)
    {
        _inputActions = Actions;
    }
    #endregion

    #region Methods
    #endregion
}