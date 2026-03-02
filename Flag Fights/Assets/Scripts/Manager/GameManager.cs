using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool gameActive;
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

    [SerializeField] AudioClip victorySFX;
    [SerializeField] AudioClip defeatSFX;

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

        StartCoroutine(StartRoundNextFrame());
    }

    IEnumerator StartRoundNextFrame()
    {
        yield return null;
        StartRound();
    }

    void Start()
    {
        if (instance != this) return;
        _roundWorld = FindObjectOfType<RoundWorldSystem>();
        StartRound();
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

    public void StartRound()
    {
        SetRound();
        Time.timeScale = 1;
        timeElapsed = false;
        timer = Time.time;
        currentTime = 0f;
        _lastDisplayedSecond = -1;
        gameActive = true;
        UIManager.Instance?.ShowHUD();
        UIManager.Instance?.UpdateScoreDisplay(_points, _enemyPoints);
        PushTimerDisplay(lossTimer);
    }

    public void NextRound()
    {
        _roundWorld?.ResetActors();
        if (EnemyPoints >= _totalPoints)
        {
            Lose();
        }
        else if (Points >= _totalPoints)
        {
            Win();
        }
        else
        {
            round++;
            UIManager.Instance?.HideAll();
            StartRound();
        }
    }

    void OnPlayerHitHandler()    => EndRound(false);
    void OnFlagCapturedHandler() => EndRound(true);

    public void EndRound(bool playerWon)
    {
        if (!gameActive) return;

        if (playerWon)
        {
            _points++;
            AudioManager.Instance?.PlaySFX(victorySFX);
        }
        else
        {
            _enemyPoints++;
            AudioManager.Instance?.PlaySFX(defeatSFX);
        }

        UIManager.Instance?.ShowScore();
        UIManager.Instance?.UpdateScoreDisplay(_points, _enemyPoints);

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