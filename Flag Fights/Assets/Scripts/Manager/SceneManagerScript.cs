using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void StartGame() => ChangeScene("Map");

    public void LoadMainMenu() => ChangeScene("MainMenu");

    private void ChangeScene(string sceneName)
    {
        FadeScreen.FadeAndLoadScene(sceneName);
    }

    public void Quit() => Application.Quit();
}
