using UnityEngine;

/// <summary>
/// SceneUIRoot de la escena MainMenu.
/// Se registra en UIManager al despertar.
/// Los botones Play y Quit se asignan en el Inspector apuntando a SceneManagerScript.
/// </summary>
public class MenuUIRoot : MonoBehaviour, ISceneUI
{
    [SerializeField] GameObject mainScreen;

    void Awake() => UIManager.Instance?.RegisterSceneUI(this);
    void OnDestroy() => UIManager.Instance?.UnregisterSceneUI(this);

    // ─── ISceneUI ────────────────────────────────────────────────────────────

    // El menú no tiene HUD ni pantallas de gameplay; los métodos son no-ops.
    public void ShowHUD()      { }
    public void ShowScore()    { }
    public void ShowGameOver() { }
    public void ShowWin()      { }

    public void HideAll() => mainScreen?.SetActive(false);

    public void UpdateScoreDisplay(int playerPoints, int enemyPoints) { }
    public void UpdateTimerDisplay(string formattedTime) { }
}
