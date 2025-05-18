using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectiveObjectsSequence : MonoBehaviour
{
    #region Fields
    [Header("Collection of the ISelected objects"), SerializeField] private GameObject[] _selectiveObjects;
    [SerializeField, HideInInspector] private int _selectiveLength = 0;
    [SerializeField, HideInInspector] private List<ISelectable> _selectables = new List<ISelectable>();
    [Header("Event on beat selected beater."), SerializeField] private UnityEvent _onSelectedBeaterBeat;
    [Header("Event on beat selected beater."), SerializeField] private UnityEvent _onSelectedBeaterHit;
    private Beater _selectedBeater;
    private ISelectable _currentSelectable;
    private int _pointerIndex = -1;
    #endregion

    #region Methods
    private void Awake ()
    {
        foreach(GameObject selective in _selectiveObjects)
        {
            ISelectable selectable = (ISelectable)ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(selective, typeof(ISelectable));

            if (selectable != null)
            {
                _selectables.Add(selectable);
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate ()
    {
        if (_selectiveObjects == null)
            return;

        if(_selectiveObjects.Length != _selectiveLength)
        {
            List<GameObject> currentTypeObjects = new List<GameObject>();

            foreach(GameObject obj in _selectiveObjects)
            {
                if(obj == null)
                    continue;

                ISelectable selectable = (ISelectable)ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(obj, typeof(ISelectable)); 

                if(selectable != null)
                {
                    currentTypeObjects.Add(obj);
                }
            }

            if(currentTypeObjects.Count > 0)
            {
                _selectiveObjects = currentTypeObjects.ToArray();
            }
        }
        
        _selectiveLength = _selectiveObjects.Length;
    }
#endif

    public void SelectNext()
    {
        int nextIndex = _pointerIndex + 1;
        SelectByIndex(nextIndex);
    }

    public void SelectPrevious()
    {
        int previousIndex = _pointerIndex - 1;
        SelectByIndex(previousIndex);
    }

    public void SelectByIndex(int Index)
    {
        if (!IndexIsValid(Index))
        {
            return;
        }

        _pointerIndex = Index;
        ISelectable selectable = _selectables[_pointerIndex];

        if (_currentSelectable == selectable)
            return;

        if (_currentSelectable != null)
            _currentSelectable.Deselect();

        _currentSelectable = _selectables[_pointerIndex];
        _selectedBeater = _currentSelectable.ReturnBeater();
        _currentSelectable.Select();
    }

    private bool IndexIsValid(int Index)
    {
        if(Index < 0 || Index >= _selectables.Count)
        {
            return false;
        }

        return true;
    }

    public void HitBySelectedBeater()
    {
        if (_selectedBeater == null)
            return;

        _selectedBeater.BeatHit();
    }

    private void OnEnable ()
    {
        if (_selectables == null || _selectables.Count == 0)
            return;

        foreach (ISelectable selectable in _selectables)
        {
            Beater beater = selectable.ReturnBeater();
            beater.OnBeat.AddListener(delegate { _onSelectedBeaterBeat?.Invoke(); });
            beater.OnHit.AddListener(delegate { _onSelectedBeaterHit?.Invoke(); });
        }
    }

    private void OnDisable ()
    {
        if (_selectables == null || _selectables.Count == 0)
            return;

        foreach (ISelectable selectable in _selectables)
        {
            Beater beater = selectable.ReturnBeater();
            beater.OnBeat.RemoveListener(delegate { _onSelectedBeaterBeat?.Invoke(); });
            beater.OnHit.RemoveListener(delegate { _onSelectedBeaterHit?.Invoke(); });
        }
    }
    #endregion
}
