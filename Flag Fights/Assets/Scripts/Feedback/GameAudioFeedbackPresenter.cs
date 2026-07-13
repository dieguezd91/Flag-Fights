using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudioFeedbackPresenter : MonoBehaviour
{
    [Header("Flag")]
    [SerializeField] AudioClip flagPickupSfx;
    [SerializeField] AudioClip flagCapturedSfx;

    [Header("Score and Round")]
    [SerializeField] AudioClip roundWinSfx;
    [SerializeField] AudioClip roundLoseSfx;

    [Header("Player")]
    [SerializeField] AudioClip playerRespawnSfx;
    [SerializeField] AudioClip playerDeathSfx;

    [Header("Music")]
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameplayMusic;

    void OnEnable()
    {
        GameEvents.OnFlagPickedUp += OnFlagPickedUp;
        GameEvents.OnFlagCaptured += OnFlagCaptured;
        GameEvents.OnRoundEnded += OnRoundEnded;
        GameEvents.OnPlayerRespawned += OnPlayerRespawned;
        GameEvents.OnPlayerHit += OnPlayerHit;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        GameEvents.OnFlagPickedUp -= OnFlagPickedUp;
        GameEvents.OnFlagCaptured -= OnFlagCaptured;
        GameEvents.OnRoundEnded -= OnRoundEnded;
        GameEvents.OnPlayerRespawned -= OnPlayerRespawned;
        GameEvents.OnPlayerHit -= OnPlayerHit;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene);
    }

    void PlayMusicForScene(Scene scene)
    {
        switch (scene.buildIndex)
        {
            case 0: AudioManager.Instance?.PlayMusic(menuMusic); break;
            case 1: AudioManager.Instance?.PlayMusic(gameplayMusic); break;
        }
    }

    void OnFlagPickedUp(PlayerController player, GameObject flag) => Play(flagPickupSfx);
    void OnFlagCaptured() => Play(flagCapturedSfx);
    void OnRoundEnded(bool playerWon) => Play(playerWon ? roundWinSfx : roundLoseSfx);
    void OnPlayerHit() => Play(playerDeathSfx);
    float _lastRespawnSfxTime = -1f;

    void OnPlayerRespawned(PlayerController player)
    {
        if (Time.time - _lastRespawnSfxTime < 0.1f) return;
        _lastRespawnSfxTime = Time.time;
        Play(playerRespawnSfx);
    }

    void Play(AudioClip clip)
    {
        AudioManager.Instance?.PlaySfx(clip);
    }
}
