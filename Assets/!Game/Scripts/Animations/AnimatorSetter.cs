using UnityEngine;

public class AnimatorSetter : MonoBehaviour
{
    #region Fields
    [Header("Animator to set parameter."), SerializeField] private Animator _animatorToSet;
    #endregion

    #region Methods
    public void SetTrigger(string TriggerName)
    {
        if (_animatorToSet == null)
            return;

        _animatorToSet.SetTrigger(TriggerName);
    }
    #endregion
}
