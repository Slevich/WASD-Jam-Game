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

    [Header("Settings of conveyor movement to beater place.")]
    [SerializeField, Range(0, 25)] private float _beaterPlaceMovementDuration = 2f;
    [SerializeField] private Transform _beaterPlaceMovementTarget;
    [SerializeField] private Ease _beaterPlaceMovementEase = Ease.Linear;

    [Header("Settings of conveyor movement to movementEndPlace.")]
    [SerializeField, Range(0, 25)] private float _endTargetMovementDuration = 2f;
    [SerializeField] private Transform _endMovementTarget;
    [SerializeField] private Ease _endTargetMovementEase = Ease.Linear;

    private List<Sequence> _sequences = new List<Sequence>();

    private float _baseDropDuration = 0;
    private float _baseRotationDuration = 0;
    private float _baseToBeaterLocationDuration = 0;
    private float _baseToEndTargetLocationDuration = 0;
    private float _timeScale = 1f;
    private bool _valuesScaled = false;
    #endregion

    #region Properties
    public float TotalMovementDuration
    {
        get
        {
            ScaleValues();
            return _dropDuration + _beaterPlaceMovementDuration;
        }
    }
    #endregion

    #region Methods
    private void Awake ()
    {
        ScaleValues();
        _timeScale = GameplaySettings.Instance.NotesSpeed;
        _dropDuration = MathF.Round(_baseDropDuration / _timeScale, 2);
        _rotationDuration = MathF.Round(_baseRotationDuration / _timeScale, 2);
        _beaterPlaceMovementDuration = MathF.Round(_baseToBeaterLocationDuration / _timeScale, 2);
        _endTargetMovementDuration = MathF.Round(_baseToEndTargetLocationDuration / _timeScale, 2);
    }

    private void ScaleValues ()
    {
        if (_valuesScaled)
            return;

        _baseDropDuration = _dropDuration;
        _baseRotationDuration = _rotationDuration;
        _baseToBeaterLocationDuration = _beaterPlaceMovementDuration;
        _baseToEndTargetLocationDuration = _endTargetMovementDuration;
        _valuesScaled = true;
    }

    public void StartMovementAnimation(GameObject MovableObject)
    {
        Transform movableObjectTransform = MovableObject.transform;
        Sequence newSequence = DOTween.Sequence();

        Tween dropTween = movableObjectTransform.DOMove(_dropTarget.position, _dropDuration);
        dropTween.SetEase(_dropEase);

        Tween moveToBeaterPlaceTween = movableObjectTransform.DOMove(_beaterPlaceMovementTarget.position, _beaterPlaceMovementDuration);
        moveToBeaterPlaceTween.SetEase(_beaterPlaceMovementEase);

        Tween rotationTween = movableObjectTransform.DORotateQuaternion(_dropTarget.rotation, _rotationDuration);
        rotationTween.SetEase(_rotationEase);

        Tween moveToEndTargetTween = movableObjectTransform.DOMove(_endMovementTarget.position, _endTargetMovementDuration);
        moveToEndTargetTween.SetEase(_endTargetMovementEase);

        newSequence.Join(dropTween).Append(moveToBeaterPlaceTween).Join(rotationTween).Append(moveToEndTargetTween);
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
