using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    public int CurrentScene;

    private Button _playBtn;
    private Button _quitBtn;

    private void Start()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        CurrentScene = SceneManager.GetActiveScene().buildIndex;

        // Solo intentamos asignar botones si estamos en la escena correspondiente (ej: Main Menu)
        if (CurrentScene == 0) BindUI();
    }

    private void BindUI()
    {
        // Asignación de botones por código buscando por nombre en la jerarquía (root de la escena o canvas)
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            _playBtn = canvas.transform.Find("MainScreen/Play Button")?.GetComponent<Button>();
            _quitBtn = canvas.transform.Find("MainScreen/Quit Button")?.GetComponent<Button>();

            if (_playBtn != null)
            {
                _playBtn.onClick.RemoveAllListeners();
                _playBtn.onClick.AddListener(() => StartGame());
            }

            if (_quitBtn != null)
            {
                _quitBtn.onClick.RemoveAllListeners();
                _quitBtn.onClick.AddListener(() => Quit());
            }
        }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;

        _playBtn?.onClick.RemoveAllListeners();
        _quitBtn?.onClick.RemoveAllListeners();
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
