using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ConveyorMovementAnimation : MonoBehaviour
{
    #region Field
    [Header("Settings of the drop on the conveyor animation.")]
    [SerializeField, Range(0, 1)] private float _dropDuration = 0.5f;
    [SerializeField] private Transform _dropTarget;
    [SerializeField] private Ease _dropEase = Ease.Linear;

    [Header("Setting of the rotation after falling on the conveyor.")]
    [SerializeField, Range(0, 1)] private float _rotationDuration = 0.2f;
    [SerializeField] private Ease _rotationEase = Ease.Linear;

    [Header("Settings of conveyor movement.")]
    [SerializeField, Range(0, 25)] private float _movementDuration = 2f;
    [SerializeField] private Transform _movementTarget;
    [SerializeField] private Ease _movementEase = Ease.Linear;

    private List<Sequence> _sequences = new List<Sequence>();

    private float _baseDropDuration = 0;
    private float _baseRotationDuration = 0;
    private float _baseMovementDuration = 0;
    private float _timeScale = 1f;
    #endregion

    #region Methods
    private void Awake ()
    {
        _baseDropDuration = _dropDuration;
        _baseRotationDuration = _rotationDuration;
        _baseMovementDuration = _movementDuration;

        _timeScale = GameplaySettings.Instance.NotesSpeed;
        _dropDuration = MathF.Round(_baseDropDuration / _timeScale, 2);
        _rotationDuration = MathF.Round(_baseRotationDuration / _timeScale, 2);
        _movementDuration = MathF.Round(_baseMovementDuration / _timeScale, 2);
    }

    public void StartMovementAnimation(GameObject MovableObject)
    {
        Transform movableObjectTransform = MovableObject.transform;
        Sequence newSequence = DOTween.Sequence();

        Tween dropTween = movableObjectTransform.DOMove(_dropTarget.position, _dropDuration);
        dropTween.SetEase(_dropEase);

        Tween moveTween = movableObjectTransform.DOMove(_movementTarget.position, _movementDuration);
        moveTween.SetEase(_movementEase);

        Tween rotationTween = movableObjectTransform.DORotateQuaternion(_dropTarget.rotation, _rotationDuration);
        rotationTween.SetEase(_rotationEase);

        newSequence.Join(dropTween).Append(moveTween).Join(rotationTween);
        newSequence.onComplete += delegate { Destroy(MovableObject); _sequences.Remove(newSequence); };
        newSequence.onKill += delegate { Destroy(MovableObject); };
        _sequences.Add(newSequence);

        newSequence.Play();
    }

    public void StopMovementAnimation()
    {
        if (_sequences == null || _sequences.Count == 0)
            return;

        foreach (Sequence sequence in _sequences)
        {

            if (sequence != null && sequence.IsPlaying())
                sequence.Kill();
        }
    }

    private void OnDisable () => StopMovementAnimation();
    #endregion
}
