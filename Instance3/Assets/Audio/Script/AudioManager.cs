using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static Action<string> OnPlaySFX;
    public static Action<string> OnPauseSFX;
    public static Action OnPauseAllSFX;
    public static Action OnStopAllSFX;

    public static Action<string> OnPlayMusic;
    public static Action OnPauseMusic;
    public static Action OnStopMusic;

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Databases")]
    [SerializeField] private SFXDatabase sfxDatabase;
    [SerializeField] private SFXDatabase musicDatabase;

    [Header("Settings")]
    [SerializeField] private int sfxPoolSize = 6;

    private AudioSource musicSource;
    private List<AudioSource> sfxSources = new();
    private Dictionary<string, AudioSource> activeSFX = new();

    private void Awake() 
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.playOnAwake = false;

        for (int i = 0; i < sfxPoolSize; i++) 
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxGroup;
            sfxSource.playOnAwake = false;
            sfxSources.Add(sfxSource);
        }
    }

    private void OnEnable() 
    {
        OnPlaySFX += HandlePlaySFX;
        OnPauseSFX += HandlePauseSFX;
        OnPauseAllSFX += HandlePauseAllSFX;
        OnStopAllSFX += HandleStopAllSFX;

        OnPlayMusic += HandlePlayMusic;
        OnPauseMusic += HandlePauseMusic;
        OnStopMusic += HandleStopMusic;
    }

    private void OnDisable() 
    {
        OnPlaySFX -= HandlePlaySFX;
        OnPauseSFX -= HandlePauseSFX;
        OnPauseAllSFX -= HandlePauseAllSFX;
        OnStopAllSFX -= HandleStopAllSFX;

        OnPlayMusic -= HandlePlayMusic;
        OnPauseMusic -= HandlePauseMusic;
        OnStopMusic -= HandleStopMusic;
    }

    private void HandlePlaySFX(string sfxName) 
    {
        var sfxAsset = sfxDatabase.GetSFX(sfxName);
        if (sfxAsset == null || sfxAsset.clip == null) return;

        var source = GetAvaibleSFXSource();
        if (source == null) return;

        source.clip = sfxAsset.clip;
        source.volume = sfxAsset.volume;
        source.loop = sfxAsset.loop;
        source.Play();

        activeSFX[sfxName] = source;
    }

    private void HandlePauseSFX(string sfxName) 
    {
        if (activeSFX.TryGetValue(sfxName, out var source) && source.isPlaying) 
        {
            source.Pause();
        }
    }

    private void HandlePauseAllSFX() 
    {
        foreach (var source in sfxSources) 
        {
            source.Pause();
        }
    }

    private void HandleStopAllSFX()
    {
        foreach (var source in sfxSources)
        {
            source.Stop();
        }

        activeSFX.Clear();
    }

    private void HandlePlayMusic(string musicName) 
    {
        var musicAsset = musicDatabase.GetSFX(musicName);
        if (musicAsset == null || musicAsset.clip == null) return;

        musicSource.clip = musicAsset.clip;
        musicSource.volume = musicAsset.volume;
        musicSource.loop = musicAsset.loop;
        musicSource.Play();
    }

    private void HandlePauseMusic() 
    {
        if (musicSource.isPlaying) 
        {
            musicSource.Pause();
        }
    }

    private void HandleStopMusic() 
    {
        if (musicSource.isPlaying) 
        {
            musicSource.Stop();
        }
    }

    private AudioSource GetAvaibleSFXSource() 
    {
        foreach (var source in sfxSources) 
        {
            if (!source.isPlaying) 
            {
                return source;
            }
        }

        return null;
    }
}
