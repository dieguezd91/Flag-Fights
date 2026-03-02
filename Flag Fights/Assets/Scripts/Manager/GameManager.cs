using System.Collections;
using System.Collections.Generic;
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

    private EnemyBase enemyBase;
    public GameObject player;
    PlayerController _playerController;
    private GameObject[] enemies;
    public GameObject Flag;
    [SerializeField] FlagSpawner flagSpawner;

    [SerializeField] Transform playerInitialTransform;

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
        if (player != null) _playerController = player.GetComponent<PlayerController>();
        if (flagSpawner == null) flagSpawner = FindObjectOfType<FlagSpawner>();
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
        _playerController = player != null ? player.GetComponent<PlayerController>() : null;
        flagSpawner = FindObjectOfType<FlagSpawner>();
        enemies = null;

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
        if (player != null && playerInitialTransform != null)
        {
            player.transform.SetPositionAndRotation(playerInitialTransform.position, playerInitialTransform.rotation);
            _playerController?.ResetRoundState();
        }

        if (flagSpawner != null)
        {
            flagSpawner.InitializeSpawner();
        }

        GetActors();
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
        ResetPreviousActors();
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

    void GetActors()
    {
        if (enemies != null && enemies.Length > 0 && enemies[0] != null)
            return;

        enemyBase = FindObjectOfType<EnemyBase>();
        enemyBase?.InitializeBase();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void ResetPreviousActors()
    {
        if (enemies == null) return;
        for (int n = 0; n < enemies.Length; n++)
        {
            if (enemies[n] == null) continue;
            enemies[n].GetComponent<GoblinController>()?.ResetEnemy();
            enemies[n].GetComponent<KnightController>()?.ResetEnemy();
        }
    }
}