using System.Collections;
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
    private TaskCompletionSource<bool> _fadeOutTcs;
    private TaskCompletionSource<bool> _fadeInTcs;

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
            
        // Initial state: invisible, non-blocking, and disabled Canvas
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;

        if (_fadeImage != null)
        {
            _fadeImage.raycastTarget = false;
        }

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }

    private void OnDisable()
    {
        CleanUpPendingTasks();
    }

    private void OnDestroy()
    {
        CleanUpPendingTasks();
    }

    private void CleanUpPendingTasks()
    {
        if (_fadeOutTcs != null && !_fadeOutTcs.Task.IsCompleted)
        {
            _fadeOutTcs.TrySetResult(false);
        }
        if (_fadeInTcs != null && !_fadeInTcs.Task.IsCompleted)
        {
            _fadeInTcs.TrySetResult(false);
        }
    }

    public Task FadeOut(float? duration = null)
    {
        // Cancel any prior fade out task
        if (_fadeOutTcs != null && !_fadeOutTcs.Task.IsCompleted)
        {
            _fadeOutTcs.TrySetResult(false);
        }

        _fadeOutTcs = new TaskCompletionSource<bool>();
        StartCoroutine(FadeOutRoutine(duration, _fadeOutTcs));
        return _fadeOutTcs.Task;
    }

    private IEnumerator FadeOutRoutine(float? duration, TaskCompletionSource<bool> tcs)
    {
        // Enable components for fading out (to black)
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = true;
        }

        if (_fadeImage != null)
        {
            _fadeImage.raycastTarget = true;
        }

        _canvasGroup.blocksRaycasts = true;

        float d = duration ?? (TweenSettings.Global != null ? TweenSettings.Global.UI.ScreenFadeDuration : 0.5f);

        Tween tween = _canvasGroup.DOFade(1f, d)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad)
            .SetLink(gameObject);

        yield return tween.WaitForCompletion();

        _canvasGroup.alpha = 1f;

        tcs.TrySetResult(true);
    }

    public Task FadeIn(float? duration = null)
    {
        // Cancel any prior fade in task
        if (_fadeInTcs != null && !_fadeInTcs.Task.IsCompleted)
        {
            _fadeInTcs.TrySetResult(false);
        }

        _fadeInTcs = new TaskCompletionSource<bool>();
        StartCoroutine(FadeInRoutine(duration, _fadeInTcs));
        return _fadeInTcs.Task;
    }

    private IEnumerator FadeInRoutine(float? duration, TaskCompletionSource<bool> tcs)
    {
        float d = duration ?? (TweenSettings.Global != null ? TweenSettings.Global.UI.ScreenFadeDuration : 0.5f);

        Tween tween = _canvasGroup.DOFade(0f, d)
            .SetUpdate(true)
            .SetEase(Ease.InQuad)
            .SetLink(gameObject);

        yield return tween.WaitForCompletion();

        // Ensure clean inactive state
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;

        if (_fadeImage != null)
        {
            _fadeImage.raycastTarget = false;
        }

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }

        tcs.TrySetResult(true);
    }

    public void FadeAndLoadScene(string sceneName, float? duration = null)
    {
        if (_isTransitioning) return;
        StartCoroutine(FadeAndLoadSceneRoutine(sceneName, duration));
    }

    private IEnumerator FadeAndLoadSceneRoutine(string sceneName, float? duration)
    {
        _isTransitioning = true;

        // 1. Fade Out (to black)
        Task fadeOutTask = FadeOut(duration);
        while (!fadeOutTask.IsCompleted)
        {
            yield return null;
        }

        // 2. Load the scene asynchronously
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        if (op != null)
        {
            op.allowSceneActivation = true;
            while (!op.isDone)
            {
                yield return null;
            }
        }

        // 3. Wait a brief moment to stabilize
        yield return new WaitForSecondsRealtime(0.1f);

        // 4. Fade In (to transparent)
        Task fadeInTask = FadeIn(duration);
        while (!fadeInTask.IsCompleted)
        {
            yield return null;
        }

        _isTransitioning = false;
    }
}