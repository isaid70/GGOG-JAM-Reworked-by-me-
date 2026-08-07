using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    private static SoundMixerManager _instance;
    public static SoundMixerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SoundMixerManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SoundMixerManager");
                    _instance = obj.AddComponent<SoundMixerManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private AudioMixer audioMixer;

    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SoundFXVolume";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        EnsureAudioMixerLoaded();
    }

    private void Start()
    {
        EnsureAudioMixerLoaded();
        LoadVolumeSettings();
    }

    private void EnsureAudioMixerLoaded()
    {
        if (audioMixer == null)
        {
            audioMixer = Resources.Load<AudioMixer>("MainMixer");
            if (audioMixer == null)
            {
                // Try finding any AudioMixer asset
                AudioMixer[] mixers = Resources.FindObjectsOfTypeAll<AudioMixer>();
                if (mixers != null && mixers.Length > 0)
                {
                    audioMixer = mixers[0];
                }
            }
        }
    }

    public void SetMasterVolume(float level)
    {
        EnsureAudioMixerLoaded();
        if (audioMixer == null)
        {
            Debug.LogWarning("[SoundMixerManager] AudioMixer reference is missing! Please assign MainMixer in Inspector.");
            return;
        }

        float db = LinearToDecibel(level);
        audioMixer.SetFloat("MasterVolume", db);
        PlayerPrefs.SetFloat(MASTER_KEY, level);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float level)
    {
        EnsureAudioMixerLoaded();
        if (audioMixer == null)
        {
            Debug.LogWarning("[SoundMixerManager] AudioMixer reference is missing! Please assign MainMixer in Inspector.");
            return;
        }

        float db = LinearToDecibel(level);
        audioMixer.SetFloat("MusicVolume", db);
        PlayerPrefs.SetFloat(MUSIC_KEY, level);
        PlayerPrefs.Save();
    }

    public void SetSoundFXVolume(float level)
    {
        EnsureAudioMixerLoaded();
        if (audioMixer == null)
        {
            Debug.LogWarning("[SoundMixerManager] AudioMixer reference is missing! Please assign MainMixer in Inspector.");
            return;
        }

        float db = LinearToDecibel(level);
        // Try standard name first, fallback to typo name if needed
        if (!audioMixer.SetFloat("SoundFXVolume", db))
        {
            audioMixer.SetFloat("SounFXVolume", db);
        }
        PlayerPrefs.SetFloat(SFX_KEY, level);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MASTER_KEY, 0.5f);
    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_KEY, 0.5f);
    public float GetSoundFXVolume() => PlayerPrefs.GetFloat(SFX_KEY, 0.5f);

    private void LoadVolumeSettings()
    {
        SetMasterVolume(GetMasterVolume());
        SetMusicVolume(GetMusicVolume());
        SetSoundFXVolume(GetSoundFXVolume());
    }

    private float LinearToDecibel(float linear)
    {
        linear = Mathf.Clamp(linear, 0.0001f, 1.0f);
        return Mathf.Log10(linear) * 20f;
    }
}


