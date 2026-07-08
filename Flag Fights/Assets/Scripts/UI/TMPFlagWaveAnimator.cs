using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public class TMPFlagWaveAnimator : MonoBehaviour
{
    static readonly int WaveAmplitudeId = Shader.PropertyToID("_WaveAmplitude");
    static readonly int WaveFrequencyId = Shader.PropertyToID("_WaveFrequency");
    static readonly int WaveSpeedId = Shader.PropertyToID("_WaveSpeed");
    static readonly int WaveVerticalAmplitudeId = Shader.PropertyToID("_WaveVerticalAmplitude");
    static readonly int WaveAnchorXId = Shader.PropertyToID("_WaveAnchorX");
    static readonly int WaveTimeId = Shader.PropertyToID("_WaveTime");

    [Header("Wave")]
    [Min(0f)] [SerializeField] private float _amplitude = 8f;
    [Min(0f)] [SerializeField] private float _frequency = 0.035f;
    [SerializeField] private float _speed = 2f;
    [Min(0f)] [SerializeField] private float _verticalAmplitude = 14f;
    [SerializeField] private float _anchorX = -400f;

    TMP_Text _text;
    Material _originalMaterial;
    Material _runtimeMaterial;

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        EnsureRuntimeMaterial();
        ApplyStaticProperties();
        SetWaveTime(Time.unscaledTime);
    }

    void Update()
    {
        if (_runtimeMaterial == null)
        {
            EnsureRuntimeMaterial();
            ApplyStaticProperties();
        }

        SetWaveTime(Time.unscaledTime);
    }

    void OnValidate()
    {
        _amplitude = Mathf.Max(0f, _amplitude);
        _frequency = Mathf.Max(0f, _frequency);
        _verticalAmplitude = Mathf.Max(0f, _verticalAmplitude);

        if (!isActiveAndEnabled || _runtimeMaterial == null)
        {
            return;
        }

        ApplyStaticProperties();
    }

    void OnDisable()
    {
        RestoreOriginalMaterial();
    }

    void OnDestroy()
    {
        RestoreOriginalMaterial();
    }

    void EnsureRuntimeMaterial()
    {
        if (_text == null)
        {
            _text = GetComponent<TMP_Text>();
        }

        if (_runtimeMaterial != null)
        {
            _text.fontMaterial = _runtimeMaterial;
            return;
        }

        _originalMaterial = _text.fontSharedMaterial;
        if (_originalMaterial == null)
        {
            return;
        }

        _runtimeMaterial = new Material(_originalMaterial)
        {
            name = $"{_originalMaterial.name} (Flag Wave Runtime)",
            hideFlags = HideFlags.DontSave
        };

        _text.fontMaterial = _runtimeMaterial;
    }

    void ApplyStaticProperties()
    {
        if (_runtimeMaterial == null)
        {
            return;
        }

        _runtimeMaterial.SetFloat(WaveAmplitudeId, _amplitude);
        _runtimeMaterial.SetFloat(WaveFrequencyId, _frequency);
        _runtimeMaterial.SetFloat(WaveSpeedId, _speed);
        _runtimeMaterial.SetFloat(WaveVerticalAmplitudeId, _verticalAmplitude);
        _runtimeMaterial.SetFloat(WaveAnchorXId, _anchorX);
    }

    void SetWaveTime(float time)
    {
        if (_runtimeMaterial != null)
        {
            _runtimeMaterial.SetFloat(WaveTimeId, time);
        }
    }

    void RestoreOriginalMaterial()
    {
        if (_text != null && _originalMaterial != null)
        {
            _text.fontMaterial = _originalMaterial;
        }

        if (_runtimeMaterial == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(_runtimeMaterial);
        }
        else
        {
            DestroyImmediate(_runtimeMaterial);
        }

        _runtimeMaterial = null;
    }
}