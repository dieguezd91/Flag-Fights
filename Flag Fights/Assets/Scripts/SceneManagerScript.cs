using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    public int currentScene;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void StartGame()
    {
        ChangeScene(1);
        Debug.Log("Game Started");
    }

    public void ChangeScene(int n)
    {
        SceneManager.LoadScene(n);
        currentScene = n;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
