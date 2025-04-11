using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MasterParam = "Master";
    private const string MusicParam = "Music";
    private const string SFXParam = "SFX";

    private const string MasterKey = "Volume_Master";
    private const string MusicKey = "Volume_Music";
    private const string SFXKey = "Volume_SFX";

    private void Start()
    {
        LoadVolume(MasterParam, masterSlider, MasterKey);
        LoadVolume(MusicParam, musicSlider, MusicKey);
        LoadVolume(SFXParam, sfxSlider, SFXKey);

        masterSlider.onValueChanged.AddListener(v => SetVolume(MasterParam, v, MasterKey));
        musicSlider.onValueChanged.AddListener(v => SetVolume(MusicParam, v, MusicKey));
        sfxSlider.onValueChanged.AddListener(v => SetVolume(SFXParam, v, SFXKey));
    }

    private void SetVolume(string parameter, float sliderValue, string prefsKey)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
        PlayerPrefs.SetFloat(prefsKey, sliderValue);
    }

    private void LoadVolume(string parameter, Slider slider, string prefsKey)
    {
        float savedValue = PlayerPrefs.GetFloat(prefsKey, 1f);
        slider.value = savedValue;

        float dB = Mathf.Log10(Mathf.Clamp(savedValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
    }
}
