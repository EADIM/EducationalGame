using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using System.IO;

public class SettingsMenu : MonoBehaviour
{
    private static SettingsMenu _instance; 

    public SettingsMenu Instance
    {
        get
        {
            if(_instance == null){
                _instance = new SettingsMenu();
            }

            return _instance;
        }
    }

    private SettingsMenu(){}

    public TMPro.TMP_Dropdown qualityLevelDropdown;
    public AudioMixer audioMixer;
    private static TextAsset default_settings;
    [SerializeField]
    private static int currentQualityIndex = 0;
    [SerializeField]
    private static int previousQualityIndex = 0;

    private void Awake() {
        default_settings = (TextAsset) Resources.Load("default_settings", typeof(TextAsset));
        try
        {
            currentQualityIndex = int.Parse(Utils.getBetween(default_settings.text, "default_quality:", "\n"));
        }
        catch(FormatException fe)
        {
            Debug.Log(fe.ToString());
            Debug.Log("Could not parse 'default_settings.txt' default quality.");
            currentQualityIndex = QualitySettings.GetQualityLevel();
        }

        previousQualityIndex = currentQualityIndex;
        QualitySettings.SetQualityLevel(currentQualityIndex);
        qualityLevelDropdown.value = currentQualityIndex;
    }

    public void SetVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex){
        previousQualityIndex = currentQualityIndex;
        currentQualityIndex = qualityIndex;
        qualityLevelDropdown.value = currentQualityIndex;
        //Debug.LogFormat("Previous Quality Index: {0}  Current Quality Index: {1}", previousQualityIndex, currentQualityIndex);
        StartCoroutine(changeDefaultSettingsFile());
    }

    private IEnumerator changeDefaultSettingsFile(){
        string oldValue = "default_quality: " + previousQualityIndex;
        string newValue = "default_quality: " + currentQualityIndex;
        string oldText = default_settings.text;
        string newText = oldText.Replace(oldValue, newValue);
        string path = Application.dataPath + "/Resources/default_settings.txt";
        //Debug.LogFormat("Default settings path: '{0}'", path);
        File.WriteAllText(path, newText);
        yield return null;
    }
}