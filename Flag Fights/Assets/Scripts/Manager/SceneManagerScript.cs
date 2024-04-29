using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    public int CurrentScene;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        CurrentScene = SceneManager.GetActiveScene().buildIndex;
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
