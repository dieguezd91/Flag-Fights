using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIRoot : MonoBehaviour, ISceneUI
{
    [Header("Panels")]
    [SerializeField] GameObject hudPanel;
    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winPanel;

    [Header("Texts")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text currentScoreText;

    [Header("Buttons")]
    [SerializeField] Button _continueButton;
    [SerializeField] Button _gameOverBackButton;
    [SerializeField] Button _winBackButton;

    void Awake()
    {
        UIManager.Instance?.RegisterSceneUI(this);
        _continueButton?.onClick.AddListener(OnContinuePressed);
        _gameOverBackButton?.onClick.AddListener(OnBackToMenuPressed);
        _winBackButton?.onClick.AddListener(OnBackToMenuPressed);
    }

    void OnDestroy()
    {
        UIManager.Instance?.UnregisterSceneUI(this);
        _continueButton?.onClick.RemoveAllListeners();
        _gameOverBackButton?.onClick.RemoveAllListeners();
        _winBackButton?.onClick.RemoveAllListeners();
    }

    void OnContinuePressed()   => GameManager.instance?.NextRound();
    void OnBackToMenuPressed() => SceneManagerScript.instance?.LoadMainMenu();

    public void ShowHUD()
    {
        HideAll();
        hudPanel?.SetActive(true);
    }

    public void ShowScore()
    {
        HideAll();
        scorePanel?.SetActive(true);
        if (currentScoreText != null && GameManager.instance != null)
            currentScoreText.text = $"{GameManager.instance.Points} - {GameManager.instance.EnemyPoints}";
    }

    public void ShowGameOver()
    {
        HideAll();
        gameOverPanel?.SetActive(true);
    }

    public void ShowWin()
    {
        HideAll();
        winPanel?.SetActive(true);
    }

    public void HideAll()
    {
        hudPanel?.SetActive(false);
        scorePanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        winPanel?.SetActive(false);
    }

    public void UpdateScoreDisplay(int playerPoints, int enemyPoints)
    {
        if (scoreText != null)
            scoreText.text = $"{playerPoints} - {enemyPoints}";
    }

    public void UpdateTimerDisplay(string formattedTime)
    {
        if (timerText != null)
            timerText.text = formattedTime;
    }
}
