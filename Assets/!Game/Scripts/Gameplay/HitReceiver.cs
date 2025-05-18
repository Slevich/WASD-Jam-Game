using UnityEngine;
using UnityEngine.Events;

public class HitReceiver : MonoBehaviour, IHitReceiving
{
    #region Fields
    [Header("Event on receiving hit."), Space (10), SerializeField] public UnityEvent _onHitReceiving;
    #endregion

    #region Properties
    public bool AlreadyHitted { get; set; } = false;
    #endregion

    #region Methods
    public void HitThis()
    {
        AlreadyHitted = true;
        _onHitReceiving?.Invoke();
    }
    #endregion
}
