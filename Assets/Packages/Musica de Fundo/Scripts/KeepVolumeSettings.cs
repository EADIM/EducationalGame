using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class KeepVolumeSettings : MonoBehaviour
{
    private float backgroundFloat = 1.0f, soundEffectsFloat = 1.0f;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    private void Awake() {
        if(SceneManager.GetActiveScene().name == "LevelSelect" || SceneManager.GetActiveScene().name == "Loading Screen")
        {
            backgroundAudio.time = VolumeManager.TimeMusic;
        }
        //ContinueSettings();
    }

    private void ContinueSettings()
    {  
        Debug.Log("Setting audio source volumes.");
        
        backgroundFloat = PlayerPrefs.GetFloat(VolumeManager.BackgroundPref, 1.0f);
        soundEffectsFloat = PlayerPrefs.GetFloat(VolumeManager.SoundEffectsPref, 1.0f);

        Debug.LogFormat("KP-BKG: {0}  KP-SFX: {1}", backgroundFloat, soundEffectsFloat);

        backgroundAudio.volume = backgroundFloat;

        if(soundEffectsAudio != null)
        {
            for (int i = 0; i < soundEffectsAudio.Length; i++)
            {
                soundEffectsAudio[i].volume = soundEffectsFloat;
            }
        }
        
    }

    public void saveTime()
    {
        VolumeManager.TimeMusic = backgroundAudio.time;
    }
}
