using UnityEngine;

[DefaultExecutionOrder(-100)]
public class TweenSettingsProvider : MonoBehaviour
{
    [Header("Global Tween Configuration")]
    [SerializeField] private TweenSettings _settings;

    private void Awake()
    {
        if (_settings == null)
        {
            Debug.LogError($"[{nameof(TweenSettingsProvider)}] TweenSettings asset is not assigned!", this);
            return;
        }

        TweenSettings.Global = _settings;
            
        Debug.Log($"[{nameof(TweenSettingsProvider)}] TweenSettings successfully initialized.", this);
    }
}
