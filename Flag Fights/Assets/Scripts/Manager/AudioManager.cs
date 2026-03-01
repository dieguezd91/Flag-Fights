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
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0: PlayMusic(_menuMusic);    break;
            case 1: PlayMusic(_gameplayMusic); break;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || _sfxSource == null) return;

        _sfxSource.PlayOneShot(clip, _sfxVolume * _masterVolume);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || _musicSource == null) return;
        if (_musicSource.clip == clip && _musicSource.isPlaying) return;
        _musicSource.clip   = clip;
        _musicSource.loop   = loop;
        _musicSource.volume = _musicVolume * _masterVolume;
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource?.Stop();

    public void SetMasterVolume(float v)
    {
        _masterVolume = Mathf.Clamp01(v);
        if (_sfxSource)   _sfxSource.volume  = _sfxVolume  * _masterVolume;
        if (_musicSource) _musicSource.volume = _musicVolume * _masterVolume;
    }

    public void SetSFXVolume(float v)
    {
        _sfxVolume = Mathf.Clamp01(v);
        if (_sfxSource) _sfxSource.volume = _sfxVolume * _masterVolume;
    }

    public void SetMusicVolume(float v)
    {
        _musicVolume = Mathf.Clamp01(v);
        if (_musicSource) _musicSource.volume = _musicVolume * _masterVolume;
    }
}
