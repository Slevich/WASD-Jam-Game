using DG.Tweening;
using UnityEngine;

public class LerpToPositionAnimation : MonoBehaviour
{
    #region Fields
    [Header("Tranform to lerp position."), SerializeField] private Transform _movingTransform;
    [Header("Moving duration."), SerializeField, Range(0, 10f)] private float _movingDuration;
    [Header("Ease type."), SerializeField] private Ease _ease = Ease.Linear;

    private Tween _currentTween;
    #endregion

    #region Methods

    public void StartAnimationLerpTo(Transform Target)
    {
        if (_movingTransform == null)
            return;

        if(_currentTween != null && _currentTween.IsPlaying())
        {
            StopAnimation();
        }

        _currentTween = _movingTransform.DOMove(Target.position, _movingDuration);
        _currentTween.Play();
    }

    public void StopAnimation()
    {
        if(_currentTween != null && _currentTween.IsPlaying())
            _currentTween.Kill();
    }

    private void OnDisable () => StopAnimation();
    #endregion
}
