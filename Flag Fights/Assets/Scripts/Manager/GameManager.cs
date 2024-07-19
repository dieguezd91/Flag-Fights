using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool gameActive;
    public int TotalPoints => _totalPoints;
    [SerializeField] int _totalPoints;
    public int Points => _points;
    int _points;
    public int EnemyPoints => _enemyPoints;
    int _enemyPoints;
    [SerializeField] public int round;
    public float timer;
    public float currentTime;
    public float stopTimer;
    [SerializeField] public float lossTimer;
    private bool timeElapsed = false; // Variable para controlar si ha transcurrido el tiempo

    // Actores
    private EnemyBase enemyBase;
    public GameObject player;
    private GameObject[] enemies;
    public GameObject Flag;
    [SerializeField] FlagSpawner flagSpawner;

    [SerializeField] Transform playerInitialTransform;

    [SerializeField] AudioClip victorySFX;
    [SerializeField] AudioClip defeatSFX;

    List<Node> _nodes;
    public List<Node> Nodes => _nodes;

    void Awake()
    {
        // Implementar Singleton correctamente
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Verificar referencias necesarias
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (flagSpawner == null) flagSpawner = FindObjectOfType<FlagSpawner>();

        _nodes = FindObjectsOfType<Node>().ToList();
    }

    void Start()
    {
        StartRound(); // Iniciar la primera ronda
    }

    private void Update()
    {
        if (gameActive)
        {
            CheckRoundStatus();
        }
    }

    public void CheckRoundStatus()
    {
        currentTime = Time.time - timer; // Calcular el tiempo transcurrido desde el inicio
        if (currentTime >= lossTimer && !timeElapsed) EndRound(false);
    }

    private void SetRound()
    {
        if (player != null && playerInitialTransform != null)
        {
            player.transform.SetPositionAndRotation(playerInitialTransform.position, playerInitialTransform.rotation);
            player.GetComponent<PlayerView>().SetFlagVisibility(false);
            player.GetComponent<PlayerModel>().HasFlag = false;
            player.GetComponent<PlayerModel>().HasUsedInvisibility = false;
        }

        if (flagSpawner != null)
        {
            flagSpawner.InitializeSpawner();
        }

        GetActors(); // Obtener las referencias de los enemigos, sus bases y la bandera
        timer = Time.time; // Iniciar el temporizador al inicio
    }

    public void StartRound()
    {
        SetRound();
        // Empieza a correr el tiempo
        Time.timeScale = 1;
        // Se activa el HUD
        UIManager.Instance?.HUD.SetActive(true);
        timeElapsed = false; // Reiniciar el indicador de tiempo transcurrido
        timer = Time.time;
        currentTime = 0f;
        gameActive = true;
    }

    public void NextRound()
    {
        DestroyPreviousActors();
        if (EnemyPoints >= _totalPoints)
        {
            Lose();
        }
        else if (Points >= _totalPoints)
        {
            Win();
        }
        else
        {
            round++;
            UIManager.Instance?.scoreScreen.SetActive(false);
            StartRound();
        }
    }

    public void EndRound(bool playerWon)
    {
        if (playerWon)
        {
            _points++; // Asignar puntos
            UIManager.Instance?.AudioSource.PlayOneShot(victorySFX); // Reproducir SFX de victoria
        }
        else
        {
            _enemyPoints++;
            UIManager.Instance?.AudioSource.PlayOneShot(defeatSFX); // Reproducir SFX de derrota
        }

        // Se desactiva el HUD
        UIManager.Instance?.HUD.SetActive(false);
        // Se actualiza el puntaje
        UIManager.Instance?.UpdateScore();
        // Se muestra una pantalla con el puntaje actual
        UIManager.Instance?.ShowScore();

        // Actualizar parámetros de juego
        Time.timeScale = 0;
        gameActive = false;
        timeElapsed = true; // Activar el indicador de tiempo transcurrido
    }

    public void Win()
    {
        // Se para el tiempo
        Time.timeScale = 0;

        UIManager.Instance?.scoreScreen.SetActive(false);
        // Se desactiva el HUD
        UIManager.Instance?.HUD.SetActive(false);
        // Mostrar pantalla de victoria
        UIManager.Instance?.winScreen.SetActive(true);

        // Volver al menú inicial
    }

    public void Lose()
    {
        // Se para el tiempo
        Time.timeScale = 0;
        UIManager.Instance?.scoreScreen.SetActive(false);
        // Se desactiva el HUD
        UIManager.Instance?.HUD.SetActive(false);
        // Mostrar pantalla de derrota
        UIManager.Instance?.gameOverScreen.SetActive(true);
    }

    void GetActors()
    {
        if (enemyBase != null)
        {
            DestroyPreviousActors();
        }
        enemyBase = FindObjectOfType<EnemyBase>();
        enemyBase?.InitializeBase();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void DestroyPreviousActors()
    {
        enemyBase = null;
        if (enemies != null)
        {
            for (int n = 0; n < enemies.Length; n++)
            {
                Destroy(enemies[n]);
            }
        }
    }
}