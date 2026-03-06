using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFadeController : MonoBehaviour
{
    public static ScreenFadeController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _fadeImage;

    private bool _isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
        DontDestroyOnLoad(gameObject);

        if (_canvasGroup == null) _canvasGroup = GetComponentInChildren<CanvasGroup>();
            
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }

    public async Task FadeOut(float? duration = null)
    {
        if (TweenSettings.Global == null) return;
        float d = duration ?? TweenSettings.Global.UI.ScreenFadeDuration;

        _canvasGroup.blocksRaycasts = true;
            
        await _canvasGroup.DOFade(1f, d)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject) 
            .AsyncWaitForCompletion();
    }

    public async Task FadeIn(float? duration = null)
    {
        if (TweenSettings.Global == null) return;
        float d = duration ?? TweenSettings.Global.UI.ScreenFadeDuration;

        await _canvasGroup.DOFade(0f, d)
            .SetUpdate(true)
            .SetEase(Ease.InQuad)
            .SetLink(gameObject)
            .AsyncWaitForCompletion();

        _canvasGroup.blocksRaycasts = false;
    }

    public async void FadeAndLoadScene(string sceneName, float? duration = null)
    {
        if (_isTransitioning) return;
        _isTransitioning = true;

        await FadeOut(duration);
            
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            await Task.Yield();
        }

        await Task.Delay(100);

        await FadeIn(duration);
            
        _isTransitioning = false;
    }
}