using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    ISceneUI _currentSceneUI;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void RegisterSceneUI(ISceneUI ui) => _currentSceneUI = ui;

    public void UnregisterSceneUI(ISceneUI ui)
    {
        if (_currentSceneUI == ui) _currentSceneUI = null;
    }

    public void ShowHUD()       => _currentSceneUI?.ShowHUD();
    public void ShowScore()     => _currentSceneUI?.ShowScore();
    public void ShowGameOver()  => _currentSceneUI?.ShowGameOver();
    public void ShowWin()       => _currentSceneUI?.ShowWin();
    public void HideAll()       => _currentSceneUI?.HideAll();

    public void UpdateScoreDisplay(int playerPoints, int enemyPoints)
        => _currentSceneUI?.UpdateScoreDisplay(playerPoints, enemyPoints);

    public void UpdateTimerDisplay(string formattedTime)
        => _currentSceneUI?.UpdateTimerDisplay(formattedTime);
}
