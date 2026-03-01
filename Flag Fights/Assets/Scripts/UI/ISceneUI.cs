public interface ISceneUI
{
    void ShowHUD();
    void ShowScore();
    void ShowGameOver();
    void ShowWin();
    void HideAll();
    void UpdateScoreDisplay(int playerPoints, int enemyPoints);
    void UpdateTimerDisplay(string formattedTime);
}
