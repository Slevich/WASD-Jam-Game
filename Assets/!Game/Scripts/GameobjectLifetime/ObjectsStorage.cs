using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

public class ObjectsStorage : MonoBehaviour
{
    #region Fields
    private ObservableCollection<GameObject> _observableObjects = new ObservableCollection<GameObject>();
    #endregion

    #region Methods
    public void AddObjectToStorage (GameObject NewStorageObject)
    {
        _observableObjects.Add(NewStorageObject);

        OnObjectDestroy onDestroy = null;
        onDestroy = (OnObjectDestroy)(ComponentsSearcher.GetSingleComponentOfTypeFromObjectAndChildren(NewStorageObject, typeof(OnObjectDestroy)));

        if (onDestroy == null)
            onDestroy = (OnObjectDestroy)(NewStorageObject.AddComponent(typeof(OnObjectDestroy)));

        onDestroy.OnDestroyCallback += delegate { _observableObjects.Remove(NewStorageObject); };
    }

    public void SubscribeOnCollectionChanged(NotifyCollectionChangedEventHandler SomeAction) => _observableObjects.CollectionChanged += SomeAction;
    #endregion
}
