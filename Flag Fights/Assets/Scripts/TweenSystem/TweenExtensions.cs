using DG.Tweening;
using UnityEngine;

public static class TweenExtensions
{
    #region Transform Animations
        
    public static void PopIn(this Transform transform, float duration = -1, DG.Tweening.Ease ease = DG.Tweening.Ease.Unset)
    {
        if (TweenSettings.Global == null) return;

        float d = duration < 0 ? TweenSettings.Global.UI.PopDuration : duration;
        DG.Tweening.Ease e = ease == DG.Tweening.Ease.Unset ? TweenSettings.Global.UI.DefaultEase : ease;

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, d).SetEase(e).SetUpdate(true).SetLink(transform.gameObject);
    }

    public static void PopOut(this Transform transform, float duration = -1, DG.Tweening.Ease ease = DG.Tweening.Ease.InBack, System.Action onComplete = null)
    {
        if (TweenSettings.Global == null) return;

        float d = duration < 0 ? TweenSettings.Global.UI.PopDuration : duration;
        transform.DOScale(Vector3.zero, d)
            .SetEase(ease)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke())
            .SetLink(transform.gameObject);
    }

    public static void PunchScale(this Transform transform, float strength = -1, float duration = -1)
    {
        if (TweenSettings.Global == null) return;

        float s = strength < 0 ? TweenSettings.Global.Gameplay.PunchStrength : strength;
        float d = duration < 0 ? TweenSettings.Global.Gameplay.PunchDuration : duration;
        transform.DOPunchScale(Vector3.one * s, d, 1, 0.5f).SetUpdate(true).SetLink(transform.gameObject);
    }

    public static void ShakePosition(this Transform transform, float strength = -1, float duration = -1)
    {
        if (TweenSettings.Global == null) return;

        float s = strength < 0 ? TweenSettings.Global.Gameplay.ShakeStrength : strength;
        float d = duration < 0 ? TweenSettings.Global.Gameplay.ShakeDuration : duration;
        transform.DOShakePosition(d, s, 10, 90, false, true).SetUpdate(true).SetLink(transform.gameObject);
    }

    #endregion

    #region UI Animations

    public static void FadeIn(this CanvasGroup canvasGroup, float duration = -1, System.Action onComplete = null)
    {
        if (TweenSettings.Global == null) return;

        float d = duration < 0 ? TweenSettings.Global.UI.FadeDuration : duration;
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, d)
            .SetEase(DG.Tweening.Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke())
            .SetLink(canvasGroup.gameObject);
    }

    public static void FadeOut(this CanvasGroup canvasGroup, float duration = -1, System.Action onComplete = null)
    {
        if (TweenSettings.Global == null) return;

        float d = duration < 0 ? TweenSettings.Global.UI.FadeDuration : duration;
        canvasGroup.DOFade(0f, d)
            .SetEase(DG.Tweening.Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke())
            .SetLink(canvasGroup.gameObject);
    }

    #endregion

    #region World Animations

    public static Tween Floating(this Transform transform, float amplitude = -1, float frequency = -1)
    {
        if (TweenSettings.Global == null) return null;

        float a = amplitude < 0 ? TweenSettings.Global.World.FloatAmplitude : amplitude;
        float f = frequency < 0 ? TweenSettings.Global.World.FloatFrequency : frequency;

        return transform.DOMoveY(transform.position.y + a, 1f / f)
            .SetEase(DG.Tweening.Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(transform.gameObject);
    }

    public static Tween RotatingIdle(this Transform transform, Vector3 rotationPerSecond)
    {
        return transform.DORotate(rotationPerSecond, 1f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetLink(transform.gameObject);
    }

    #endregion
}