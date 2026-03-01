using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la carga de escenas. Los botones de cada escena llaman
/// directamente a estos métodos desde el Inspector (sin BindUI por código).
/// </summary>
public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    public int CurrentScene;

    private void Start()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        CurrentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    public void StartGame() => ChangeScene(1);

    public void LoadMainMenu() => ChangeScene(0);

    private void ChangeScene(int n)
    {
        SceneManager.LoadScene(n);
        CurrentScene = n;
    }

    public void Quit() => Application.Quit();
}
