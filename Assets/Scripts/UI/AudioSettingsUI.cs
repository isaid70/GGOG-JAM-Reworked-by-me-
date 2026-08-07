using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable()
    {
        InitializeSliders();
    }

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        if (SoundMixerManager.Instance == null)
            return;

        if (masterSlider != null)
        {
            masterSlider.value = SoundMixerManager.Instance.GetMasterVolume();
            masterSlider.onValueChanged.RemoveListener(OnMasterSliderChanged);
            masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        }

        if (musicSlider != null)
        {
            musicSlider.value = SoundMixerManager.Instance.GetMusicVolume();
            musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = SoundMixerManager.Instance.GetSoundFXVolume();
            sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
        }
    }

    public void OnMasterSliderChanged(float value)
    {
        if (SoundMixerManager.Instance != null)
        {
            SoundMixerManager.Instance.SetMasterVolume(value);
        }
    }

    public void OnMusicSliderChanged(float value)
    {
        if (SoundMixerManager.Instance != null)
        {
            SoundMixerManager.Instance.SetMusicVolume(value);
        }
    }

    public void OnSFXSliderChanged(float value)
    {
        if (SoundMixerManager.Instance != null)
        {
            SoundMixerManager.Instance.SetSoundFXVolume(value);
        }
    }
}
