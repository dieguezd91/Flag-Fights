using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] public int totalPoints = 3;
    [SerializeField] public int points = 0;
    [SerializeField] public int enemyPoints = 0;
    public float timer;
    public float currentTime;
    public float stopTimer;
    [SerializeField] public float lossTimer;
    private bool timeElapsed = false; // Variable para controlar si ha transcurrido el tiempo

    void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        timer = Time.time; // Iniciar el temporizador al inicio
    }

    void Update()
    {
        CheckRoundStatus();
    }

    public void CheckRoundStatus()
    {
        currentTime = Time.time - timer; // Calcular el tiempo transcurrido desde el inicio

        if (currentTime >= lossTimer && !timeElapsed)
        {
            LoseRound();
            Debug.Log("LOSE ROUND");
            timeElapsed = true; // Activar el indicador de tiempo transcurrido
        }
        else if (enemyPoints >= totalPoints)
        {
            Lose();
            Debug.Log("LOSER");
        }
    }

    public void WinRound()
    {
        timeElapsed = false; // Reiniciar el indicador de tiempo transcurrido
        points++;
        RestartRound();
        //Actualizar marcador
    }

    public void LoseRound()
    {
        timeElapsed = false; // Reiniciar el indicador de tiempo transcurrido
        enemyPoints ++;
        //Actualizar marcador
        RestartRound();
    }

    public void Win()
    {
        //Mostrar pantalla de victoria
        //Volver al menu inicial
    }

    public void Lose()
    {
        //Mostrar antalla de derrota
        //Volver al menu inicial
    }

    public void RestartRound()
    {
        timer = Time.time;
        currentTime = 0f;
    }
}
