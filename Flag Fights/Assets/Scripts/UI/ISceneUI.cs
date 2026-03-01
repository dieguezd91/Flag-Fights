/// <summary>
/// Contrato que implementa cada SceneUIRoot.
/// UIManager delega en esta interfaz sin conocer la estructura interna de los paneles.
/// </summary>
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
