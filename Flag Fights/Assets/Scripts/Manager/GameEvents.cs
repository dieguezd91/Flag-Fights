using System;

public static class GameEvents
{
    public static event Action OnPlayerHit;
    public static event Action OnFlagCaptured;

    public static void RaisePlayerHit()    => OnPlayerHit?.Invoke();
    public static void RaiseFlagCaptured() => OnFlagCaptured?.Invoke();
}
