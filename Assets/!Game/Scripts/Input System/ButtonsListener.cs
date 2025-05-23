using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using System.Linq;
using UnityEditor.UIElements;
using UniRx;

public class ButtonsListener : MonoBehaviour
{
    [Header("Events on buttons"), SerializeField] private EventOnButton[] _listeners;

    private void Awake ()
    {
        foreach (EventOnButton listener in _listeners)
        {
            InputHandler.PlayerInputActionsUpdate
                .Where(actions => actions.Any(action => action.name == listener.ActionName))
                .Subscribe(actions => 
                {
                    InputAction listenerAction = actions.Where(action => action.name == listener.ActionName).First();
                    listener.StartToListen(listenerAction); 
                })
                .AddTo(this);
        }
    }
}

[Serializable]
public class EventOnButton
{
    [Header("Called when button pressed (single time)."), SerializeField] public UnityEvent _onButtonPressed;
    [Header("Called when button released (single time)."), SerializeField] public UnityEvent _onButtonReleased;
    [SerializeField, HideInInspector, ExecuteInEditMode] public string _actionName = string.Empty;

    public string ActionName => _actionName;
    private bool _isPressed = false;

    public void StartToListen(InputAction action)
    {
        bool inProgress = action.inProgress;

        if (inProgress && !_isPressed)
        {
            _onButtonPressed?.Invoke();
        }
        else if(!inProgress && _isPressed)
        {
            _onButtonReleased?.Invoke();
        }

        _isPressed = inProgress;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EventOnButton))]
public class EventOnButtonDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EventOnButton eventOnButton = null;
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
        eventOnButton = (EventOnButton)property.boxedValue;
        //PropertyInfo selectedNameProperty = targetObjectClassType.GetProperty(property.propertyPath, flags);
        //if (selectedNameProperty != null)
        //{
        //    eventOnButton = (EventOnButton)selectedNameProperty.GetValue(targetObject);
        //}

        InputSystem_Actions.PlayerActionsActions inputActions = InputHandler.PlayerActions;



        PropertyInfo[] properties = inputActions.GetType().GetProperties(flags);
        IEnumerable<PropertyInfo> buttonsProperties = properties.Where(prop => prop.PropertyType ==  typeof(InputAction));
        
        List<string> buttonsPropertiesNames = new List<string>();
        
        if(buttonsProperties != null && buttonsProperties.Count() > 0)
        {
            buttonsPropertiesNames = buttonsProperties.Select(prop => prop.Name).ToList();
        }

        if(buttonsPropertiesNames.Count > 0 && eventOnButton != null)
        {
            SerializedProperty actionNameProperty = property.FindPropertyRelative("_actionName");
            string savedName = actionNameProperty.stringValue;
            int index = buttonsPropertiesNames.IndexOf(savedName);

            if (index < 0)
                index = 0;

            index = EditorGUILayout.Popup("Enable buttons.", index, buttonsPropertiesNames.ToArray());
            actionNameProperty.stringValue = buttonsPropertiesNames[index];
        }

        SerializedProperty pressedEventProperty = property.FindPropertyRelative("_onButtonPressed");
        SerializedProperty releasedEventProperty = property.FindPropertyRelative("_onButtonReleased");

        EditorGUILayout.PropertyField(pressedEventProperty, new GUIContent("Event on button pressed."));
        EditorGUILayout.PropertyField(releasedEventProperty, new GUIContent("Event on button released."));;
        EditorGUI.EndProperty();
    }
}
#endif