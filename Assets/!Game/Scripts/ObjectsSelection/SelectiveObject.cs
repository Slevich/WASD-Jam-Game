using UnityEngine;
using UnityEngine.Events;

public class SelectiveObject : MonoBehaviour, ISelectable
{
    #region Fields
    [Header("Is currently selected?"), SerializeField, ReadOnly] private bool _selected = false;
    [Header("Selected beater."), SerializeField] private Beater _selectedBeater;
    [Header("Event on selection."), SerializeField] private UnityEvent _onSelection;
    [Header("Event on deselection."), SerializeField] private UnityEvent _onDeselection;


    #endregion

    #region Methods
    public void Select()
    {
        if (_selected)
            return;

        _onSelection?.Invoke();
        _selected = true;
    }

    public void Deselect()
    {
        if (!_selected)
            return;

        _onDeselection?.Invoke();
        _selected = false;
    }

    public Beater ReturnBeater() => _selectedBeater;
    #endregion
}
