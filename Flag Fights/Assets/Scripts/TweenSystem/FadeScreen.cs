using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _fadeImage;
    private Canvas _canvas;

    private void Awake()
    {
        if (_canvasGroup == null) _canvasGroup = GetComponentInChildren<CanvasGroup>();
        if (_fadeImage == null) _fadeImage = GetComponentInChildren<Image>();
        _canvas = GetComponent<Canvas>();
        
        Hide();
    }

    public void Show()
    {
        if (_canvas != null) _canvas.enabled = true;
        SetAlpha(1f);
        SetBlocking(true);
    }

    public void Hide()
    {
        SetAlpha(0f);
        SetBlocking(false);
        if (_canvas != null) _canvas.enabled = false;
    }

    public void SetAlpha(float alpha)
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = alpha;
        }
    }

    public void SetBlocking(bool value)
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = value;
            _canvasGroup.interactable = value;
        }
        if (_fadeImage != null)
        {
            _fadeImage.raycastTarget = value;
        }
    }

    public CanvasGroup CanvasGroup => _canvasGroup;
}
