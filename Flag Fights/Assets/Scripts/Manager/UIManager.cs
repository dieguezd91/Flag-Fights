using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    [Header("UI Text")]
    public TextMeshProUGUI score;
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI timer;

    [Header("UI Panels")]
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject HUD;
    public GameObject scoreScreen;

    [Header("Buttons")]
    [SerializeField] private Button _restartGameOverBtn;
    [SerializeField] private Button _quitGameOverBtn;
    [SerializeField] private Button _restartWinBtn;
    [SerializeField] private Button _quitWinBtn;
    [SerializeField] private Button _continueScoreBtn;

    public void Start()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        _audioSource = GetComponent<AudioSource>();

        BindUI();
    }

    private void BindUI()
    {
        if (gameOverScreen != null)
        {
            _restartGameOverBtn = gameOverScreen.transform.Find("BackButton")?.GetComponent<Button>();
            
            if (_restartGameOverBtn != null)
            {
                _restartGameOverBtn.onClick.RemoveAllListeners();
                _restartGameOverBtn.onClick.AddListener(() => SceneManagerScript.instance?.LoadMainMenu());
            }
        }

        if (winScreen != null)
        {
            _restartWinBtn = winScreen.transform.Find("BackButton")?.GetComponent<Button>();

            if (_restartWinBtn != null)
            {
                _restartWinBtn.onClick.RemoveAllListeners();
                _restartWinBtn.onClick.AddListener(() => SceneManagerScript.instance?.LoadMainMenu());
            }
        }

        if (scoreScreen != null)
        {
            _continueScoreBtn = scoreScreen.transform.Find("Continue Button")?.GetComponent<Button>();

            if (_continueScoreBtn != null)
            {
                _continueScoreBtn.onClick.RemoveAllListeners();
                _continueScoreBtn.onClick.AddListener(() => GameManager.instance?.NextRound());
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        
        _restartGameOverBtn?.onClick.RemoveAllListeners();
        _restartWinBtn?.onClick.RemoveAllListeners();
        _continueScoreBtn?.onClick.RemoveAllListeners();
    }

    public void Update()
    {        
        UpdateTimer();
        UpdateScore();
    }

    public void UpdateScore()
    {
        score.text = GameManager.instance.Points.ToString() + " - " + GameManager.instance.EnemyPoints.ToString();
    }

    public void UpdateTimer()
    {
        int minutesLeft = Mathf.FloorToInt((GameManager.instance.lossTimer - GameManager.instance.currentTime) / 60.0f);
        int secondsLeft = Mathf.FloorToInt((GameManager.instance.lossTimer - GameManager.instance.currentTime) % 60.0f);

        timer.text = string.Format("{0:00}:{1:00}", minutesLeft, secondsLeft);
    }

    public void ShowScore()
    {
        scoreScreen.SetActive(true);
        currentScore.text = GameManager.instance.Points.ToString() + " - " + GameManager.instance.EnemyPoints.ToString();
    }

    public void RestartScore()
    {

    }
}
