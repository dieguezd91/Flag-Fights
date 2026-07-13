using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioSource _musicSource;

    [Header("Music Clips")]
    [SerializeField] AudioClip _menuMusic;
    [SerializeField] AudioClip _gameplayMusic;

    [Header("Volume")]
    [Range(0f, 1f)] [SerializeField] float _masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] float _sfxVolume    = 1f;
    [Range(0f, 1f)] [SerializeField] float _musicVolume  = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        ApplyVolumes();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (Instance != this)
            return;

        PlayMusicForScene(SceneManager.GetActiveScene());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene);
    }

    private void PlayMusicForScene(Scene scene)
    {
        switch (scene.buildIndex)
        {
            case 0: PlayMusic(_menuMusic);    break;
            case 1: PlayMusic(_gameplayMusic); break;
        }
    }

    private void ApplyVolumes()
    {
        if (_sfxSource != null)
            _sfxSource.volume = _sfxVolume * _masterVolume;

        if (_musicSource != null)
            _musicSource.volume = _musicVolume * _masterVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || _sfxSource == null) return;

        _sfxSource.PlayOneShot(clip);
    }

    public void PlaySfx(AudioClip clip) => PlaySFX(clip);

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || _musicSource == null) return;
        if (_musicSource.clip == clip && _musicSource.isPlaying) return;
        _musicSource.clip   = clip;
        _musicSource.loop   = loop;
        ApplyVolumes();
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource?.Stop();

    public void SetMasterVolume(float v)
    {
        _masterVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    public void SetSFXVolume(float v)
    {
        _sfxVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    public void SetMusicVolume(float v)
    {
        _musicVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _masterVolume = Mathf.Clamp01(_masterVolume);
        _sfxVolume = Mathf.Clamp01(_sfxVolume);
        _musicVolume = Mathf.Clamp01(_musicVolume);
        if (Application.isPlaying)
        {
            ApplyVolumes();
        }
    }
#endif
}
