using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTweenFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _hoverScale = -1;
    [SerializeField] private float _clickScale = -1;
    [SerializeField] private float _duration = -1;
    [SerializeField] private DG.Tweening.Ease _ease = DG.Tweening.Ease.OutBack;

    private Vector3 _originalScale;
    private DG.Tweening.Tween _currentTween;

    private void Awake()
    {
        _originalScale = transform.localScale;
            
        if (TweenSettings.Global == null) return;

        if (_hoverScale < 0) _hoverScale = TweenSettings.Global.UI.ButtonHoverScale;
        if (_clickScale < 0) _clickScale = TweenSettings.Global.UI.ButtonClickScale;
        if (_duration < 0) _duration = TweenSettings.Global.FastDuration;
    }

    private void OnDisable()
    {
        _currentTween?.Kill();
        transform.localScale = _originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTo(_originalScale * _hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleTo(_originalScale);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ScaleTo(_originalScale * _clickScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ScaleTo(_originalScale * _hoverScale);
    }

    private void ScaleTo(Vector3 targetScale)
    {
        _currentTween?.Kill();
        _currentTween = transform.DOScale(targetScale, _duration)
            .SetEase(_ease)
            .SetUpdate(true)
            .SetLink(gameObject);
    }
}