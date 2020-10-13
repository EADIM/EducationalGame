using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager _instance;

    public VolumeManager Instance{
        get
        {
            if(_instance == null){
                _instance = new VolumeManager();
            }

            return _instance;
        } 
    }

    private VolumeManager(){}
    private int firstPlayInt;

    public static readonly string BackgroundPref = "BackgroundPref";
    public static readonly string SoundEffectsPref = "SoundEffectsPref";
    [SerializeField]
    private static float startClip = .3f;
    private float backgroundFloat, soundEffectsFloat;
    static public float TimeMusic = .0f;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    private void Awake() {
        //Debug.Log("VM - Awake");
        backgroundAudio.time = TimeMusic;
        if(soundEffectsAudio.Length > 0)
        {
            soundEffectsAudio[0].time = soundEffectsAudio[0].clip.length * startClip;
        }
        VolumeManager.DontDestroyOnLoad(this);
    }

    
    void Start()
    {
        //Debug.Log("VM - Start");

        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref, 1.0f);
        soundEffectsFloat = PlayerPrefs.GetFloat(BackgroundPref, 1.0f);

        Debug.LogFormat("VM-BKG: {0} VM-SFX: {1}", backgroundFloat, soundEffectsFloat);
    }
}
