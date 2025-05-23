using UnityEngine;
using UnityEngine.Events;

public class EventsSwitch : MonoBehaviour
{
    [Header("Switch state."), SerializeField, ReadOnly] private bool _switch = false;
    [Header("Event on swith is true."), SerializeField] private UnityEvent OnTrue;
    [Header("Event on swith is false."), SerializeField] private UnityEvent OnFalse;

    public void SwitchToTrue()
    {
        if (_switch)
            return;

        _switch = true;
        OnTrue?.Invoke();
    }

    public void SwitchToFalse ()
    {
        if (!_switch)
            return;

        _switch = false;
        OnFalse?.Invoke();
    }

    public void Switch()
    {
        if(!_switch)
            SwitchToTrue();
        else 
            SwitchToFalse();
    }
}
