using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
using UniRx;
using System.Reflection;
using System.Linq;

public static class InputHandler
{
    #region Fields
    private static InputSystem_Actions _inputActions;
    public static Subject<InputAction[]> PlayerInputActionsUpdate;
    //private static TouchInfo _touchInfo;
    private static ActionUpdate _update;

    private static Action _updateAction;

    private static IInputInfo[] _info;
    #endregion

    #region Properties
    static bool InputActionsIsNull
    {
        get
        {
            if (_inputActions != null)
                return false;
            else
            {
                Debug.LogError("Input actions is null!");
                return true;
            }
        }
    }

    public static InputSystem_Actions.PlayerActionsActions PlayerActions => _inputActions.PlayerActions;
    #endregion

    #region Constructor
    static InputHandler()
    {
        //Debug.Log("Инициализация инпута!");
        _inputActions = new InputSystem_Actions();

        if (Application.isPlaying)
            Initialize();
    }
    #endregion

    #region Methods
    public static void Initialize()
    {
        PlayerInputActionsUpdate = new Subject<InputAction[]>();
        _update = new ActionUpdate();

        _info = new IInputInfo[]
        {
            new MouseKeyboardInfo(_inputActions)
        };

        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
        PropertyInfo[] actionsProperties = _inputActions.PlayerActions.GetType().GetProperties(flags);

        if(actionsProperties == null || actionsProperties.Length == 0)
        {
            return;
        }

        List<InputAction> playerInputActions = new List<InputAction>();

        foreach( PropertyInfo property in actionsProperties)
        {
            playerInputActions.Add(property.GetValue(_inputActions.PlayerActions) as InputAction);
        }

        _updateAction = delegate 
        {
            //Debug.Log("Апдейт инпута!");
            _inputActions.PlayerActions.GetType().GetProperties();
            PlayerInputActionsUpdate.OnNext(playerInputActions.ToArray());
        };
    }

    public static void EnableInput()
    {
        _inputActions.Enable();
        _update.StartUpdate(_updateAction);

        //_touchInfo.SubscribeActions();
    }

    public static void DisableInput()
    {
        _inputActions.Disable();
        _update.StopUpdate();

        //_touchInfo.DisposeActions();
    }
    #endregion
}

