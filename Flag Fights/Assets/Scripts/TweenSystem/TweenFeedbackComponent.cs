using DG.Tweening;
using UnityEngine;

public class TweenFeedbackComponent : MonoBehaviour
{
    public enum AnimationType { Floating, Rotating, Pulse }

    [SerializeField] private AnimationType _type = AnimationType.Floating;
    [SerializeField] private float _strength = -1f;
    [SerializeField] private float _duration = -1f;
    [SerializeField] private DG.Tweening.Ease _ease = DG.Tweening.Ease.InOutSine;

    private DG.Tweening.Tween _currentTween;

    private void Awake()
    {
        if (TweenSettings.Global == null) return;

        if (_duration < 0)
        {
            if (_type == AnimationType.Floating) _duration = 1f / TweenSettings.Global.World.FloatFrequency;
            else _duration = TweenSettings.Global.DefaultDuration;
        }

        if (_strength < 0)
        {
            if (_type == AnimationType.Floating) _strength = TweenSettings.Global.World.FloatAmplitude;
            else _strength = TweenSettings.Global.UI.PopInScale - 1f;
        }
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    private void OnDisable()
    {
        _currentTween?.Kill();
    }

    private void StartAnimation()
    {
        _currentTween?.Kill();

        switch (_type)
        {
            case AnimationType.Floating:
                _currentTween = transform.DOMoveY(transform.position.y + _strength, _duration)
                    .SetEase(_ease)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(gameObject);
                break;
            case AnimationType.Rotating:
                _currentTween = transform.DORotate(new Vector3(0, 360, 0), _duration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Incremental)
                    .SetLink(gameObject);
                break;
            case AnimationType.Pulse:
                _currentTween = transform.DOScale(transform.localScale * (1f + _strength), _duration)
                    .SetEase(_ease)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(gameObject);
                break;
        }
    }
}