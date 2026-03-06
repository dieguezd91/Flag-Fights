using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TweenSettings", menuName = "FlagFights/TweenSettings")]
public class TweenSettings : ScriptableObject
{
    public static TweenSettings Global { get; internal set; }

    [Header("General Defaults")]
    public float DefaultDuration = 0.3f;
    public float FastDuration = 0.15f;
    public float SlowDuration = 0.6f;

    [Space]
    public UIConfig UI;
    public GameplayConfig Gameplay;
    public WorldConfig World;

    [Serializable]
    public class UIConfig
    {
        public float PopDuration = 0.3f;
        public float PopInScale = 1.2f;
        public float FadeDuration = 0.25f;
        public float ScreenFadeDuration = 0.5f;
        public float ButtonHoverScale = 1.1f;
        public float ButtonClickScale = 0.95f;
        public DG.Tweening.Ease DefaultEase = DG.Tweening.Ease.OutBack;
    }

    [Serializable]
    public class GameplayConfig
    {
        public float PunchStrength = 0.2f;
        public float PunchDuration = 0.15f;
        public float ShakeStrength = 0.1f;
        public float ShakeDuration = 0.2f;
    }

    [Serializable]
    public class WorldConfig
    {
        public float FloatAmplitude = 0.2f;
        public float FloatFrequency = 2f;
        public float SpawnPopDuration = 0.5f;
        public DG.Tweening.Ease SpawnEase = DG.Tweening.Ease.OutElastic;
    }
}