using UnityEngine;
using UnityEngine.UI;

public class MenuUIRoot : MonoBehaviour, ISceneUI
{
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject creditsPanel;

    [Header("Buttons")]
    [SerializeField] Button _playButton;
    [SerializeField] Button _creditsButton;
    [SerializeField] Button _creditsBackButton;
    [SerializeField] Button _quitButton;

    void Awake()
    {
        UIManager.Instance?.RegisterSceneUI(this);
        _playButton?.onClick.AddListener(OnPlayPressed);
        _creditsButton?.onClick.AddListener(OnCreditsPressed);
        _creditsBackButton?.onClick.AddListener(OnCreditsBackPressed);
        _quitButton?.onClick.AddListener(OnQuitPressed);
    }

    void OnDestroy()
    {
        UIManager.Instance?.UnregisterSceneUI(this);
        _playButton?.onClick.RemoveAllListeners();
        _creditsButton?.onClick.RemoveAllListeners();
        _creditsBackButton?.onClick.RemoveAllListeners();
        _quitButton?.onClick.RemoveAllListeners();
    }

    void OnPlayPressed()        => SceneManagerScript.instance?.StartGame();
    void OnQuitPressed()        => SceneManagerScript.instance?.Quit();
    void OnCreditsPressed()     { mainScreen?.SetActive(false); creditsPanel?.SetActive(true);  }
    void OnCreditsBackPressed() { creditsPanel?.SetActive(false); mainScreen?.SetActive(true);  }

    public void ShowHUD()      { }
    public void ShowScore()    { }
    public void ShowGameOver() { }
    public void ShowWin()      { }

    public void HideAll() => mainScreen?.SetActive(false);

    public void UpdateScoreDisplay(int playerPoints, int enemyPoints) { }
    public void UpdateTimerDisplay(string formattedTime) { }
}
