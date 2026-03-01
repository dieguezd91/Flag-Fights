using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            return; // Evitar que el duplicado siga ejecutando
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Verificar referencias necesarias
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (flagSpawner == null) flagSpawner = FindObjectOfType<FlagSpawner>();

        _nodes = FindObjectsOfType<Node>().ToList();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        flagSpawner = FindObjectOfType<FlagSpawner>();
        _nodes = FindObjectsOfType<Node>().ToList();
        enemies = null; // Forzar re-fetch de enemigos en la nueva escena

        // Resetear estado para nueva partida
        _points = 0;
        _enemyPoints = 0;
        round = 0;
        GoblinController.CurrentLeader = null;

        StartCoroutine(StartRoundNextFrame());
    }

    IEnumerator StartRoundNextFrame()
    {
        yield return null; // Esperar un frame para que UIManager.Start() corra primero
        StartRound();
    }

    void Start()
    {
        if (instance != this) return; // Evitar que el duplicado inicie una ronda
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
        UIManager.Instance?.ShowHUD();
        timeElapsed = false; // Reiniciar el indicador de tiempo transcurrido
        timer = Time.time;
        currentTime = 0f;
        gameActive = true;
    }

    public void NextRound()
    {
        ResetPreviousActors();
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
            UIManager.Instance?.HideAll();
            StartRound();
        }
    }

    public void EndRound(bool playerWon)
    {
        if (playerWon)
        {
            _points++;
            UIManager.Instance?.AudioSource.PlayOneShot(victorySFX);
        }
        else
        {
            _enemyPoints++;
            UIManager.Instance?.AudioSource.PlayOneShot(defeatSFX);
        }

        // Mostrar pantalla de puntaje (oculta el HUD internamente)
        UIManager.Instance?.ShowScore();
        UIManager.Instance?.UpdateScoreDisplay(_points, _enemyPoints);

        Time.timeScale = 0;
        gameActive = false;
        timeElapsed = true;
    }

    public void Win()
    {
        Time.timeScale = 0;
        UIManager.Instance?.ShowWin();
    }

    public void Lose()
    {
        Time.timeScale = 0;
        UIManager.Instance?.ShowGameOver();
    }

    void GetActors()
    {
        // Si los enemigos ya existen (ronda 2+), fueron reseteados: no instanciar de nuevo
        if (enemies != null && enemies.Length > 0 && enemies[0] != null)
            return;

        enemyBase = FindObjectOfType<EnemyBase>();
        enemyBase?.InitializeBase();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void ResetPreviousActors()
    {
        if (enemies == null) return;
        for (int n = 0; n < enemies.Length; n++)
        {
            if (enemies[n] == null) continue;
            var goblin = enemies[n].GetComponent<GoblinController>();
            goblin?.ResetEnemy();
        }
    }
}