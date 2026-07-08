using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFadeController : MonoBehaviour
{
    public static ScreenFadeController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private FadeScreen _fadeScreen;

    private bool _isTransitioning;
    private Coroutine _fadeCoroutine;
    private Tween _currentTween;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log($"[{nameof(ScreenFadeController)}] Duplicate detected, destroying: {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
        DontDestroyOnLoad(gameObject);

        if (_fadeScreen == null)
        {
            _fadeScreen = GetComponentInChildren<FadeScreen>();
            if (_fadeScreen == null)
            {
                _fadeScreen = GetComponent<FadeScreen>();
                if (_fadeScreen == null)
                {
                    _fadeScreen = gameObject.AddComponent<FadeScreen>();
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
        }
    }

    public void FadeOut(float? duration = null)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeOutRoutine(duration));
    }

    public IEnumerator FadeOutRoutine(float? duration = null)
    {
        Debug.Log("[ScreenFadeController] FadeToBlack started");
        
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill(false);
        }

        if (_fadeScreen != null)
        {
            _fadeScreen.Show();
            _fadeScreen.SetAlpha(0f);
        }

        float d = duration ?? (TweenSettings.Global != null ? TweenSettings.Global.UI.ScreenFadeDuration : 0.5f);

        if (_fadeScreen != null && _fadeScreen.CanvasGroup != null)
        {
            _currentTween = _fadeScreen.CanvasGroup.DOFade(1f, d)
                .SetUpdate(true) // Ignore timeScale
                .SetEase(Ease.OutQuad)
                .SetLink(gameObject);

            while (_currentTween != null && _currentTween.IsActive() && !_currentTween.IsComplete())
            {
                yield return null;
            }
        }

        if (_fadeScreen != null)
        {
            _fadeScreen.SetAlpha(1f);
            _fadeScreen.SetBlocking(true);
        }

        Debug.Log("[ScreenFadeController] FadeToBlack completed");
        _fadeCoroutine = null;
    }

    public void FadeIn(float? duration = null)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeInRoutine(duration));
    }

    public IEnumerator FadeInRoutine(float? duration = null)
    {
        Debug.Log("[ScreenFadeController] FadeFromBlack started");

        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill(false);
        }

        if (_fadeScreen != null)
        {
            _fadeScreen.SetAlpha(1f);
            _fadeScreen.SetBlocking(true);
        }

        float d = duration ?? (TweenSettings.Global != null ? TweenSettings.Global.UI.ScreenFadeDuration : 0.5f);

        if (_fadeScreen != null && _fadeScreen.CanvasGroup != null)
        {
            _currentTween = _fadeScreen.CanvasGroup.DOFade(0f, d)
                .SetUpdate(true) // Ignore timeScale
                .SetEase(Ease.InQuad)
                .SetLink(gameObject);

            while (_currentTween != null && _currentTween.IsActive() && !_currentTween.IsComplete())
            {
                yield return null;
            }
        }

        if (_fadeScreen != null)
        {
            _fadeScreen.Hide();
        }

        Debug.Log("[ScreenFadeController] FadeFromBlack completed");
        _fadeCoroutine = null;
    }

    public void FadeAndLoadScene(string sceneName, float? duration = null)
    {
        if (_isTransitioning) return;
        StartCoroutine(FadeAndLoadSceneRoutine(sceneName, duration));
    }

    public IEnumerator FadeAndLoadSceneRoutine(string sceneName, float? duration = null)
    {
        _isTransitioning = true;
        Debug.Log($"[ScreenFadeController] FadeAndLoadScene started for: {sceneName}");

        yield return StartCoroutine(FadeOutRoutine(duration));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        if (op != null)
        {
            op.allowSceneActivation = true;
            while (!op.isDone)
            {
                yield return null;
            }
        }

        yield return new WaitForSecondsRealtime(0.1f);

        yield return StartCoroutine(FadeInRoutine(duration));

        Debug.Log($"[ScreenFadeController] FadeAndLoadScene completed for: {sceneName}");
        _isTransitioning = false;
    }
}