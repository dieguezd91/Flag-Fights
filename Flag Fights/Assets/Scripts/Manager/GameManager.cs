using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool gameActive;
    private bool _isRoundEnding = false;
    [SerializeField, Min(0f)] private float _playerDeathResultDelay = 2.8f;
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

    public void StartRound()
    {
        _isRoundEnding = false;
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
        GameEvents.RaiseRoundStarted(round);
    }

    public async void NextRound()
    {
        // 1. Fade out to black
        await FadeScreen.FadeOut();

        // 2. Perform the reset while the screen is black
        _roundWorld?.ResetActors();
        
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
            StartRound();
        }

        // 3. Optional small delay for polish
        await Task.Delay(200);

        // 4. Fade back in
        await FadeScreen.FadeIn();
    }

    void OnPlayerHitHandler()
    {
        if (_isRoundEnding) return;
        _isRoundEnding = true;

        if (player != null)
        {
            player.GetComponent<PlayerController>()?.Die();
        }

        StartCoroutine(DelayEndRound(false));
    }

    private IEnumerator DelayEndRound(bool playerWon)
    {
        yield return new WaitForSecondsRealtime(_playerDeathResultDelay);
        EndRound(playerWon);
    }

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
