using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown qualityLevelDropdown;
    public AudioMixer BKGMixer, SFXMixer;
    public Slider BKGSlider, SFXSlider;
    public TMPro.TMP_Text BKGPercentage, SFXPercentage;
    public Toggle InvertControlsToggle;

    private static float backgroundFloat, sfxFloat;
    private static int qualityIndex;

    private void Start() {
        BKGSlider.value = AudioManager.BKG_Volume;
        SFXSlider.value = AudioManager.SFX_Volume;

        SetMusicVolume(AudioManager.BKG_Volume);
        SetSFXVolume(AudioManager.SFX_Volume);
        SetQuality(GraphicsManager.Quality_Index);
        SetInvertedControls(ControlsManager.InvertedControls);
    }

    public void SetText(float value, TMPro.TMP_Text tmp_text)
    {
        int percentage = (int) ((value + 80.0f) * (100/80.0f));
        string text = percentage.ToString() + " %";
        //Debug.LogFormat("{0}: {1}", tmp_text.transform.parent.name,text);
        tmp_text.text = text;
    }

    public void SetMusicVolume(float volume){
        BKGMixer.SetFloat("volume", volume);
        SetText(volume, BKGPercentage);
        AudioManager.BKG_Volume = volume;
    }

    public void SetSFXVolume(float volume){
        SFXMixer.SetFloat("volume", volume);
        SetText(volume, SFXPercentage);
        AudioManager.SFX_Volume = volume;
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityLevelDropdown.value = qualityIndex;
        GraphicsManager.Quality_Index = qualityIndex;
    }

    public void SetInvertedControls(bool set)
    {
        ControlsManager.InvertedControls = set;
        InvertControlsToggle.isOn = set;
    }

    public void SaveSettings()
    {
        Debug.LogFormat("Saving setting:\n\tBKG: {0}  \n\tSFX:{1}  \n\tQuality: {2}", AudioManager.BKG_Volume, AudioManager.SFX_Volume, GraphicsManager.Quality_Index);
        AudioManager.Save();
        GraphicsManager.Save();
        ControlsManager.Save();
    }
}