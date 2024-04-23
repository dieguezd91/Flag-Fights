using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool gameActive;
    [SerializeField] public int totalPoints;
    [SerializeField] public int points;
    [SerializeField] public int enemyPoints;
    [SerializeField] public int round;
    public float timer;
    public float currentTime;
    public float stopTimer;
    [SerializeField] public float lossTimer;
    private bool timeElapsed = false; // Variable para controlar si ha transcurrido el tiempo

    //Actores
    private PlayerBase playerBase;
    private EnemyBase enemyBase;
    private GameObject player;
    private GameObject[] enemies;
    private GameObject flag;

    [SerializeField] Transform playerInitialTransform;

    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(instance);

        player = GameObject.FindGameObjectWithTag("Player");

        flag = GameObject.FindGameObjectWithTag("Flag");

        StartRound(); // Iniciar la primera ronda
    }

    void Update()
    {
        if(gameActive) CheckRoundStatus();
    }

    public void CheckRoundStatus()
    {
        currentTime = Time.time - timer; // Calcular el tiempo transcurrido desde el inicio

        if (currentTime >= lossTimer && !timeElapsed) EndRound(false);
        else if (enemyPoints >= totalPoints) Lose();        
    }

    private void SetRound()
    {
        player.transform.SetPositionAndRotation(playerInitialTransform.position, playerInitialTransform.rotation);
        GetActors();            // Obtener las referencias de los enemigos, sus bases y la bandera
        timer = Time.time;      // Iniciar el temporizador al inicio
        flag.SetActive(true);   // Activar bandera de mapa
    }

    public void StartRound()
    {
        SetRound();
        //Empieza a correr el tiempo
        Time.timeScale = 1;
        //Se activa el hud
        UIManager.Instance.HUD.SetActive(true);
        timeElapsed = false; // Reiniciar el indicador de tiempo transcurrido
        timer = Time.time;
        currentTime = 0f;
        gameActive = true;
    }

    public void NextRound()
    {
        DestroyPreviousActors();
        round++;
        UIManager.Instance.scoreScreen.SetActive(false);
        StartRound();
    }

    public void EndRound(bool playerWon)
    {
        if (playerWon) points++;        //Asignar puntos
        else enemyPoints++;

        //Se desactiva el hud
        UIManager.Instance.HUD.SetActive(false);
        //Se actualiza el puntaje
        UIManager.Instance.UpdateScore();
        //Se muestra una pantalla con el puntaje actual
        UIManager.Instance.ShowScore();

        //Actualizar parámetros de juego
        Time.timeScale = 0;
        gameActive = false;
        timeElapsed = true; // Activar el indicador de tiempo transcurrido
    }

    public void Win()
    {
        //Mostrar pantalla de victoria
        UIManager.Instance.winScreen.SetActive(true);
        //Se desactiva el hud
        UIManager.Instance.HUD.SetActive(false);
        //Se para el tiempo
        Time.timeScale = 0;

        //Volver al menu inicial
    }

    public void Lose()
    {
        //Mostrar pantalla de derrota
        UIManager.Instance.gameOverScreen.SetActive(true);
        //Se desactiva el hud
        UIManager.Instance.HUD.SetActive(false);
        //Se para el tiempo
        Time.timeScale = 0;
    }

    void GetActors()
    {
        enemyBase = FindObjectOfType<EnemyBase>();
        enemyBase.InitializeBase();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void DestroyPreviousActors()
    {
        enemyBase = null;
        for (int n = 0; n < enemies.Length; n++) Destroy(enemies[n]);
    }
}