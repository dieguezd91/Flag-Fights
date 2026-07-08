using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnPlayerHit;
    public static event Action OnFlagCaptured;
    public static event Action<int> OnRoundStarted;
    public static event Action<bool> OnRoundEnded;
    public static event Action<bool> OnRoundEndSequenceStarted;
    public static event Action<GameObject> OnFlagSpawned;
    public static event Action<PlayerController, GameObject> OnFlagPickedUp;
    public static event Action<PlayerController, bool> OnFlagCarryChanged;
    public static event Action<int, int> OnScoreChanged;
    public static event Action<PlayerController> OnPlayerRespawned;

    public static void RaisePlayerHit() => OnPlayerHit?.Invoke();
    public static void RaiseFlagCaptured() => OnFlagCaptured?.Invoke();
    public static void RaiseRoundStarted(int round) => OnRoundStarted?.Invoke(round);
    public static void RaiseRoundEnded(bool playerWon) => OnRoundEnded?.Invoke(playerWon);
    public static void RaiseRoundEndSequenceStarted(bool playerWon) => OnRoundEndSequenceStarted?.Invoke(playerWon);
    public static void RaiseFlagSpawned(GameObject flag) => OnFlagSpawned?.Invoke(flag);
    public static void RaiseFlagPickedUp(PlayerController player, GameObject flag) => OnFlagPickedUp?.Invoke(player, flag);
    public static void RaiseFlagCarryChanged(PlayerController player, bool hasFlag) => OnFlagCarryChanged?.Invoke(player, hasFlag);
    public static void RaiseScoreChanged(int playerPoints, int enemyPoints) => OnScoreChanged?.Invoke(playerPoints, enemyPoints);
    public static void RaisePlayerRespawned(PlayerController player) => OnPlayerRespawned?.Invoke(player);
}
