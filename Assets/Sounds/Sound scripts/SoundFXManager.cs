using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Persistent Audio Sources")]
    [SerializeField] private AudioSource flux;
    [SerializeField] private AudioSource Background;
    [SerializeField] private AudioSource BossTheme;
    [SerializeField] private AudioSource Mines;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Automatically route persistent AudioSources if assigned
        AssignGroupIfPresent(Background, musicGroup);
        AssignGroupIfPresent(BossTheme, musicGroup);
        AssignGroupIfPresent(flux, sfxGroup);
        AssignGroupIfPresent(Mines, sfxGroup);
    }

    private void AssignGroupIfPresent(AudioSource source, AudioMixerGroup group)
    {
        if (source != null && group != null)
        {
            source.outputAudioMixerGroup = group;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (audioClip == null || soundFXObject == null) return;

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;

        if (sfxGroup != null)
        {
            audioSource.outputAudioMixerGroup = sfxGroup;
        }

        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}

