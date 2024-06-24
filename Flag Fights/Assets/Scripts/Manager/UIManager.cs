using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    public TextMeshProUGUI score;
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI timer;

    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject HUD;
    public GameObject scoreScreen;

    public void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        _audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        
        UpdateTimer();
        UpdateScore();
    }

    public void UpdateScore()
    {
        score.text = GameManager.instance.Points.ToString() + " - " + GameManager.instance.EnemyPoints.ToString();
    }

    public void UpdateTimer()
    {
        int minutesLeft = Mathf.FloorToInt((GameManager.instance.lossTimer - GameManager.instance.currentTime) / 60.0f);
        int secondsLeft = Mathf.FloorToInt((GameManager.instance.lossTimer - GameManager.instance.currentTime) % 60.0f);

        timer.text = string.Format("{0:00}:{1:00}", minutesLeft, secondsLeft);
    }

    public void ShowScore()
    {
        scoreScreen.SetActive(true);
        currentScore.text = GameManager.instance.Points.ToString() + " - " + GameManager.instance.EnemyPoints.ToString();
    }

    public void RestartScore()
    {

    }
}
