using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool gameActive;
    private bool _isRoundEnding = false;
    private bool _isTransitioningRound;
    public bool IsRoundEnding => _isRoundEnding;
    [SerializeField, Min(0f)] private float _playerDeathResultDelay = 2.8f;
    [SerializeField, Min(0f)] private float _enemyDefeatResultDelay = 2.0f;
    public int TotalPoints => _totalPoints;
    [SerializeField] int _totalPoints;
    public int Points => _points;
    int _points;
    public int EnemyPoints => _enemyPoints;
    int _enemyPoints;
    [SerializeField] public int round;
    public float timer;
    public float currentTime;
    public float stopTimer;
    [SerializeField] public float lossTimer;
    private bool timeElapsed = false;

    public GameObject player;
    public GameObject Flag;

    RoundWorldSystem _roundWorld;

    int _lastDisplayedSecond = -1;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEvents.OnPlayerHit    += OnPlayerHitHandler;
        GameEvents.OnFlagCaptured += OnFlagCapturedHandler;

        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.OnPlayerHit    -= OnPlayerHitHandler;
        GameEvents.OnFlagCaptured -= OnFlagCapturedHandler;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _roundWorld = FindObjectOfType<RoundWorldSystem>();

        _points = 0;
        _enemyPoints = 0;
        round = 0;
        GoblinController.CurrentLeader = null;

        PrepareRound();
        StartCoroutine(StartRoundNextFrame());
    }

    IEnumerator StartRoundNextFrame()
    {
        yield return null;
        if (ScreenFadeController.Instance != null)
        {
            while (ScreenFadeController.Instance.IsTransitioning)
            {
                yield return null;
            }
        }
        ActivateRound();
    }

    void Start()
    {
        if (instance != this) return;
        _roundWorld = FindObjectOfType<RoundWorldSystem>();
        // Relying on StartRoundNextFrame() from OnSceneLoaded to trigger the first round's StartRound() after fade-in
    }

    private void Update()
    {
        if (gameActive)
        {
            CheckRoundStatus();
        }
    }

    public void CheckRoundStatus()
    {
        if (_isRoundEnding) return;

        currentTime = Time.time - timer;
        if (currentTime >= lossTimer && !timeElapsed)
        {
            EndRound(false);
            return;
        }

        float remaining = Mathf.Max(0f, lossTimer - currentTime);
        int second = Mathf.FloorToInt(remaining);
        if (second != _lastDisplayedSecond)
        {
            _lastDisplayedSecond = second;
            PushTimerDisplay(remaining);
        }
    }

    void PushTimerDisplay(float remaining)
    {
        int m = Mathf.FloorToInt(remaining / 60f);
        int s = Mathf.FloorToInt(remaining % 60f);
        UIManager.Instance?.UpdateTimerDisplay(string.Format("{0:00}:{1:00}", m, s));
    }

    private void SetRound()
    {
        _roundWorld?.SetupRound();
        timer = Time.time;
    }

    public void PrepareRound()
    {
        _isRoundEnding = false;
        _roundWorld?.SetupRound();
    }

    public void ActivateRound()
    {
        Time.timeScale = 1;
        timeElapsed = false;
        timer = Time.time;
        currentTime = 0f;
        _lastDisplayedSecond = -1;
        gameActive = true;
        UIManager.Instance?.ShowHUD();
        UIManager.Instance?.UpdateScoreDisplay(_points, _enemyPoints);
        PushTimerDisplay(lossTimer);
        GameEvents.RaiseRoundStarted(round);
    }

    public void StartRound()
    {
        PrepareRound();
        ActivateRound();
    }

    public void NextRound()
    {
        if (_isTransitioningRound) return;
        StartCoroutine(NextRoundRoutine());
    }

    private IEnumerator NextRoundRoutine()
    {
        _isTransitioningRound = true;
        Debug.Log("[GameManager] NextRound started");

        // 1. Fade out to black (ignore timescale)
        if (ScreenFadeController.Instance != null)
        {
            yield return StartCoroutine(ScreenFadeController.Instance.FadeOutRoutine());
        }

        // 2. Perform the reset while the screen is black
        _roundWorld?.ResetActors();
        
        bool roundStarted = false;
        if (_enemyPoints >= _totalPoints)
        {
            Lose();
        }
        else if (_points >= _totalPoints)
        {
            Win();
        }
        else
        {
            round++;
            UIManager.Instance?.HideAll();
            PrepareRound();
            roundStarted = true;
        }

        // 3. Optional small delay for polish (independent of Time.timeScale)
        yield return new WaitForSecondsRealtime(0.2f);

        // 4. Fade back in
        if (ScreenFadeController.Instance != null)
        {
            yield return StartCoroutine(ScreenFadeController.Instance.FadeInRoutine());
        }

        if (roundStarted)
        {
            yield return new WaitForSecondsRealtime(0.3f); // Small extra delay for camera focus
            ActivateRound();
        }

        Debug.Log("[GameManager] NextRound completed");
        _isTransitioningRound = false;
    }

    private void BeginRoundEndSequence(bool playerWon)
    {
        if (_isRoundEnding) return;
        _isRoundEnding = true;

        GameEvents.RaiseRoundEndSequenceStarted(playerWon);
        StartCoroutine(DelayEndRound(playerWon));
    }

    void OnPlayerHitHandler()
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>()?.Die();
        }

        BeginRoundEndSequence(false);
    }

    private IEnumerator DelayEndRound(bool playerWon)
    {
        float delay = playerWon ? _enemyDefeatResultDelay : _playerDeathResultDelay;
        yield return new WaitForSecondsRealtime(delay);
        EndRound(playerWon);
    }

    void OnFlagCapturedHandler() => BeginRoundEndSequence(true);

    public void EndRound(bool playerWon)
    {
        if (!gameActive) return;

        if (playerWon)
        {
            _points++;
        }
        else
        {
            _enemyPoints++;
        }

        GameEvents.RaiseScoreChanged(_points, _enemyPoints);

        UIManager.Instance?.ShowScore();
        UIManager.Instance?.UpdateScoreDisplay(_points, _enemyPoints);

        GameEvents.RaiseRoundEnded(playerWon);

        Time.timeScale = 0;
        gameActive = false;
        timeElapsed = true;
    }

    public void Win()
    {
        Time.timeScale = 0;
        UIManager.Instance?.ShowWin();
    }

    public void Lose()
    {
        Time.timeScale = 0;
        UIManager.Instance?.ShowGameOver();
    }

}
