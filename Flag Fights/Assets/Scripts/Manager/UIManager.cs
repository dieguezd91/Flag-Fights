using UnityEngine;

/// <summary>
/// Singleton persistente entre escenas. Orquesta la UI sin conocer paneles individuales.
/// Delega toda responsabilidad visual al ISceneUI registrado por la escena actual.
/// El audio se gestiona exclusivamente a través de AudioManager.
/// </summary>
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

    // ─── Registro ────────────────────────────────────────────────────────────

    /// <summary>Llamado desde SceneUIRoot.Awake() al cargar cada escena.</summary>
    public void RegisterSceneUI(ISceneUI ui) => _currentSceneUI = ui;

    /// <summary>Llamado desde SceneUIRoot.OnDestroy() al salir de cada escena.</summary>
    public void UnregisterSceneUI(ISceneUI ui)
    {
        if (_currentSceneUI == ui) _currentSceneUI = null;
    }

    // ─── API de pantallas ────────────────────────────────────────────────────

    public void ShowHUD()       => _currentSceneUI?.ShowHUD();
    public void ShowScore()     => _currentSceneUI?.ShowScore();
    public void ShowGameOver()  => _currentSceneUI?.ShowGameOver();
    public void ShowWin()       => _currentSceneUI?.ShowWin();
    public void HideAll()       => _currentSceneUI?.HideAll();

    // ─── API de datos ────────────────────────────────────────────────────────

    public void UpdateScoreDisplay(int playerPoints, int enemyPoints)
        => _currentSceneUI?.UpdateScoreDisplay(playerPoints, enemyPoints);

    public void UpdateTimerDisplay(string formattedTime)
        => _currentSceneUI?.UpdateTimerDisplay(formattedTime);
}
