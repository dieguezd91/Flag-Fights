using UnityEngine;

public class GameAudioFeedbackPresenter : MonoBehaviour
{
    [Header("Flag")]
    [SerializeField] AudioClip flagPickupSfx;
    [SerializeField] AudioClip flagCapturedSfx;

    [Header("Score and Round")]
    [SerializeField] AudioClip roundWinSfx;
    [SerializeField] AudioClip roundLoseSfx;

    [Header("Player")]
    [SerializeField] AudioClip playerHitSfx;
    [SerializeField] AudioClip playerRespawnSfx;

    void OnEnable()
    {
        GameEvents.OnFlagPickedUp += OnFlagPickedUp;
        GameEvents.OnFlagCaptured += OnFlagCaptured;
        GameEvents.OnRoundEnded += OnRoundEnded;
        GameEvents.OnPlayerHit += OnPlayerHit;
        GameEvents.OnPlayerRespawned += OnPlayerRespawned;
    }

    void OnDisable()
    {
        GameEvents.OnFlagPickedUp -= OnFlagPickedUp;
        GameEvents.OnFlagCaptured -= OnFlagCaptured;
        GameEvents.OnRoundEnded -= OnRoundEnded;
        GameEvents.OnPlayerHit -= OnPlayerHit;
        GameEvents.OnPlayerRespawned -= OnPlayerRespawned;
    }

    void OnFlagPickedUp(PlayerController player, GameObject flag) => Play(flagPickupSfx);
    void OnFlagCaptured() => Play(flagCapturedSfx);
    void OnRoundEnded(bool playerWon) => Play(playerWon ? roundWinSfx : roundLoseSfx);
    void OnPlayerHit() => Play(playerHitSfx);
    void OnPlayerRespawned(PlayerController player) => Play(playerRespawnSfx);

    void Play(AudioClip clip)
    {
        AudioManager.Instance?.PlaySfx(clip);
    }
}
