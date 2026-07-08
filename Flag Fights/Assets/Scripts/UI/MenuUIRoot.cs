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
        _creditsButton?.onClick.AddListener(OpenCredits);
        _creditsBackButton?.onClick.AddListener(CloseCredits);
        _quitButton?.onClick.AddListener(OnQuitPressed);

        // Initial setup
        if (mainScreen != null) mainScreen.transform.localScale = Vector3.one;
        if (creditsPanel != null) creditsPanel.SetActive(false);
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

    public void OpenCredits()
    {
        mainScreen?.transform.PopOut(onComplete: () =>
        {
            mainScreen.SetActive(false);
            creditsPanel.SetActive(true);
            creditsPanel.transform.PopIn();
        });
    }

    public void CloseCredits()
    {
        creditsPanel?.transform.PopOut(onComplete: () =>
        {
            creditsPanel.SetActive(false);
            mainScreen.SetActive(true);
            mainScreen.transform.PopIn();
        });
    }

    public void ShowHUD()      { }
    public void ShowScore()    { }
    public void ShowGameOver() { }
    public void ShowWin()      { }

    public void HideAll() => mainScreen?.transform.PopOut(onComplete: () => mainScreen.SetActive(false));

    public void UpdateScoreDisplay(int playerPoints, int enemyPoints) { }
    public void UpdateTimerDisplay(string formattedTime) { }
}
