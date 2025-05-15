using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputInfo
{
    #region Fields
    protected InputSystem_Actions _inputActions;
    #endregion

    #region Properties
    protected bool ActionsIsNull
    {
        get
        {
            if (_inputActions == null)
            {
                Debug.LogError("Input actions in MouseKeyboardInfo is null!");
                return true;
            }
            else return true;
        }
    }
    #endregion

    #region Constructor
    public InputInfo (InputSystem_Actions Actions)
    {
        _inputActions = Actions;
    }
    #endregion
}
