using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteColorAnimation : MonoBehaviour
{
    #region Fields
    [Header("Sprite renderer to animate."), SerializeField] private SpriteRenderer _spriteRenderer;
    [Header("Materials colors."), SerializeField] private Color[] _colors;
    [Header("Animation duration."), SerializeField, Range(0f, 10f)] private float _duration = 1f;
    [Header("Ease type."), SerializeField] public Ease _easeType = Ease.Linear;

    private Tween _currentTween;
    private Color _startColor;
    #endregion

    #region Methods
    private void Awake ()
    {
        if (_spriteRenderer != null)
            _startColor = _spriteRenderer.color;
    }

    public void LerpToColorByIndex (int ColorIndex)
    {
        if (ColorIndex < 0 || ColorIndex >= _colors.Length)
        {
            Debug.LogError("Index of the color is out of range!");
            return;
        }

        if (_spriteRenderer == null)
            return;

        if (_currentTween != null && _currentTween.IsPlaying())
            StopAnimation();

        Color color = _colors[ColorIndex];
        Tween colorTween = _spriteRenderer.DOColor(color, _duration).SetEase(_easeType);
        colorTween.Play();
    }

    public void SetColorByIndex(int ColorIndex)
    {
        if (ColorIndex < 0 || ColorIndex >= _colors.Length)
        {
            Debug.LogError("Index of the color is out of range!");
            return;
        }

        if (_spriteRenderer == null)
            return;

        Color color = _colors[ColorIndex];
        _spriteRenderer.color = color;
    }

    public void StopAnimation()
    {
        if (_currentTween != null && _currentTween.IsPlaying())
            _currentTween.Kill();
    }

    private void OnDisable ()
    {
        StopAnimation();

        _spriteRenderer.color = _startColor;
    }
    #endregion
}
