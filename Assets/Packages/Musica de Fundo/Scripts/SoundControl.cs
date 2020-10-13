using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    AudioManager AudioManager;
    public static bool isButtonMute;
    public static bool isBackgroundMute;

    private void Awake() {
        AudioManager = Object.FindObjectOfType<AudioManager>();
        if(AudioManager == null){
            Debug.Log("Audio Manager is null.");
        }
        else{
            //Debug.Log("Audio Manager is assigned.");
        }
    }

    public void playButtonSound()
    {
        AudioManager.ButtonAudio.time = AudioManager.ButtonAudio.clip.length * AudioManager.startButtonClip;
        AudioManager.ButtonAudio.Play();
        isButtonMute = false;
    }

    public void stopButtonSound()
    {
        AudioManager.ButtonAudio.Stop();
        isButtonMute = true;
    }

    public void playBackgroundSound()
    {
        AudioManager.BackgroundAudio.Play();
        isBackgroundMute = false;
    }

    public void stopBackgroundSound()
    {
        AudioManager.BackgroundAudio.Stop();
        isBackgroundMute = true;
    }
}
