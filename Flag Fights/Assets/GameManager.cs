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
    [SerializeField] public int round = 0;
    public float timer;
    public float currentTime;
    public float stopTimer;
    [SerializeField] public float lossTimer;
    private bool timeElapsed = false; // Variable para controlar si ha transcurrido el tiempo

    public GameObject flag;

    // Posiciones iniciales del jugador y el enemigo
    public Transform playerInitialPosition;
    public Transform enemyInitialPosition;
    public Transform flagInitialPosition;
    private Transform playerTransform;
    private Transform enemyTransform;
    private Transform flagTransform;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        // Asignar las posiciones iniciales
        playerInitialPosition = GameObject.Find("PlayerInitialPosition").transform;
        enemyInitialPosition = GameObject.Find("EnemyInitialPosition").transform;
        flagInitialPosition = GameObject.Find("FlagInitialPosition").transform;

        // Obtener las referencias a las transformadas del jugador, el enemigo y la bandera
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTransform = GameObject.FindGameObjectWithTag("Enemy").transform;
        flagTransform = GameObject.FindGameObjectWithTag("Flag").transform;

        timer = Time.time; // Iniciar el temporizador al inicio
        //UIManager.Instance.RestartScore();
        RestartRound(); // Iniciar la primera ronda
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
        points++;
        UIManager.Instance.HUD.SetActive(false);
        UIManager.Instance.UpdateScore();
        UIManager.Instance.ShowScore();
        //Se para el tiempo
        Time.timeScale = 0;
    }

    public void LoseRound()
    {
        //El enemigo suma un punto
        enemyPoints++;
        //Se desactiva el hud
        UIManager.Instance.HUD.SetActive(false);
        //Se actualiza el puntaje
        UIManager.Instance.UpdateScore();
        //Se muestra una pantalla con el puntaje actual
        UIManager.Instance.ShowScore();
        //Se para el tiempo
        Time.timeScale = 0;
    }

    public void NextRound()
    {
        round++;
        UIManager.Instance.scoreScreen.SetActive(false);
        RestartRound();
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

    public void RestartRound()
    {
        flag.SetActive(true);
        // Reiniciar posiciones del jugador, el enemigo y la bandera a las posiciones iniciales
        playerTransform.position = playerInitialPosition.position;
        enemyTransform.position = enemyInitialPosition.position;
        flagTransform.position = flagInitialPosition.position;
        //Empieza a correr el tiempo
        Time.timeScale = 1;
        //Se activa el hud
        UIManager.Instance.HUD.SetActive(true);
        timeElapsed = false; // Reiniciar el indicador de tiempo transcurrido
        timer = Time.time;
        currentTime = 0f;

    }
}
