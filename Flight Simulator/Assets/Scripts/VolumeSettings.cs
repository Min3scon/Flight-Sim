using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] string musicParam = "MusicVolume";   // exposed Music volume parameter
    [SerializeField] string sfxParam = "SFXVolume";       // exposed SFX volume parameter

    [Header("UI (auto-found by hierarchy names if left empty)")]
    [SerializeField] Slider musicSlider;       // Settings/Panel/Music
    [SerializeField] Slider sfxSlider;         // Settings/Panel/Sound Effects

    const string MusicPref = "MusicVolume";
    const string SfxPref   = "SFXVolume";

    void Awake()
    {
        AutoAssignSliders();
        LoadPrefs();
        ApplyAll();
        HookEvents();
    }

    void AutoAssignSliders()
    {
        if (musicSlider == null)
        {
            var musicObj = transform.Find("Panel/Music");
            if (musicObj != null) musicSlider = musicObj.GetComponentInChildren<Slider>();
        }

        if (sfxSlider == null)
        {
            var sfxObj = transform.Find("Panel/Sound Effects");
            if (sfxObj != null) sfxSlider = sfxObj.GetComponentInChildren<Slider>();
        }
    }

    void HookEvents()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    void LoadPrefs()
    {
        float music = PlayerPrefs.GetFloat(MusicPref, 1f);
        float sfx   = PlayerPrefs.GetFloat(SfxPref,   1f);

        if (musicSlider != null) musicSlider.value = music;
        if (sfxSlider   != null) sfxSlider.value   = sfx;
    }

    void ApplyAll()
    {
        if (musicSlider != null) SetMusicVolume(musicSlider.value);
        if (sfxSlider   != null) SetSfxVolume(sfxSlider.value);
    }

    // Slider value expected in [0,1]. Map to dB for mixer.
    void SetMusicVolume(float value)
    {
        if (mixer != null && !string.IsNullOrEmpty(musicParam))
            mixer.SetFloat(musicParam, SliderToDb(value));

        PlayerPrefs.SetFloat(MusicPref, value);
    }

    void SetSfxVolume(float value)
    {
        if (mixer != null && !string.IsNullOrEmpty(sfxParam))
            mixer.SetFloat(sfxParam, SliderToDb(value));

        PlayerPrefs.SetFloat(SfxPref, value);
    }

    float SliderToDb(float value)
    {
        // Clamp to avoid log(0); -80dB is near silence
        return value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
    }
}