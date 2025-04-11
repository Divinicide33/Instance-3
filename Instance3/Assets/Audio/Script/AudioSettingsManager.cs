using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Volume Labels")]
    [SerializeField] private TMP_Text masterLabel;
    [SerializeField] private TMP_Text musicLabel;
    [SerializeField] private TMP_Text sfxLabel;

    [Header("Reset")]
    [SerializeField] private Button resetButton;

    private const string MasterParam = "Master";
    private const string MusicParam = "Music";
    private const string SFXParam = "SFX";

    private const string MasterKey = "Volume_Master";
    private const string MusicKey = "Volume_Music";
    private const string SFXKey = "Volume_SFX";

    private void Start()
    {
        LoadVolume(MasterParam, masterSlider, MasterKey, masterLabel);
        LoadVolume(MusicParam, musicSlider, MusicKey, musicLabel);
        LoadVolume(SFXParam, sfxSlider, SFXKey, sfxLabel);

        masterSlider.onValueChanged.AddListener(v => SetVolume(MasterParam, v, MasterKey, masterLabel));
        musicSlider.onValueChanged.AddListener(v => SetVolume(MusicParam, v, MusicKey, musicLabel));
        sfxSlider.onValueChanged.AddListener(v => SetVolume(SFXParam, v, SFXKey, sfxLabel));

        resetButton.onClick.AddListener(ResetToDefault);
    }

    private void SetVolume(string parameter, float sliderValue, string prefsKey, TMP_Text label)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
        PlayerPrefs.SetFloat(prefsKey, sliderValue);
        UpdateVolumeLabel(label, sliderValue);
    }

    private void LoadVolume(string parameter, Slider slider, string prefsKey, TMP_Text label)
    {
        float savedValue = PlayerPrefs.GetFloat(prefsKey, 1f);
        slider.value = savedValue;

        float dB = Mathf.Log10(Mathf.Clamp(savedValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);

        UpdateVolumeLabel(label, savedValue);
    }

    private void UpdateVolumeLabel(TMP_Text label, float sliderValue)
    {
        label.text = Mathf.RoundToInt(sliderValue * 100f) + "%";
    }

    private void ResetToDefault()
    {
        SetVolume(MasterParam, 1f, MasterKey, masterLabel);
        SetVolume(MusicParam, 1f, MusicKey, musicLabel);
        SetVolume(SFXParam, 1f, SFXKey, sfxLabel);

        masterSlider.value = 1f;
        musicSlider.value = 1f;
        sfxSlider.value = 1f;
    }
}
