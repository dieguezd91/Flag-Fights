using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI score;
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI timer;

    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject HUD;
    public GameObject scoreScreen;

    public void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    public void Update()
    {
        
        UpdateTimer();
        UpdateScore();
    }

    public void UpdateScore()
    {
        score.text = GameManager.instance.points.ToString() + " - " + GameManager.instance.enemyPoints.ToString();
    }

    public void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(GameManager.instance.currentTime / 60.0f);
        int seconds = Mathf.FloorToInt(GameManager.instance.currentTime % 60.0f);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowScore()
    {
        scoreScreen.SetActive(true);
        currentScore.text = GameManager.instance.points.ToString() + " - " + GameManager.instance.enemyPoints.ToString();
    }

    public void RestartScore()
    {

    }
}
