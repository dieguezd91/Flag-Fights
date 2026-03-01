using UnityEngine;

public class MenuUIRoot : MonoBehaviour, ISceneUI
{
    [SerializeField] GameObject mainScreen;

    void Awake() => UIManager.Instance?.RegisterSceneUI(this);
    void OnDestroy() => UIManager.Instance?.UnregisterSceneUI(this);

    public void ShowHUD()      { }
    public void ShowScore()    { }
    public void ShowGameOver() { }
    public void ShowWin()      { }

    public void HideAll() => mainScreen?.SetActive(false);

    public void UpdateScoreDisplay(int playerPoints, int enemyPoints) { }
    public void UpdateTimerDisplay(string formattedTime) { }
}
