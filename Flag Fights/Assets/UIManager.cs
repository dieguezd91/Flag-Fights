using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI score;
    public TextMeshProUGUI timer;

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
        score.text = GameManager.Instance.points.ToString() + " - " + GameManager.Instance.enemyPoints.ToString();
    }

    public void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(GameManager.Instance.currentTime / 60.0f);
        int seconds = Mathf.FloorToInt(GameManager.Instance.currentTime % 60.0f);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartScore()
    {

    }
}
