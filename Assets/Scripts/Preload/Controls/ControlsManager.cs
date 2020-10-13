using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static readonly string InvertedControlsPref = "Inverted Controls";
    public static readonly string LeftSensitivityPref = "Left Joystick Sensitivity";
    public static readonly string RightSensitivityPref = "Right Joystick Sensitivity";
    public static bool InvertedControls;
    public static float LeftSensitivity;
    public static float RightSensitivity;

    private void Awake() 
    {
        InvertedControls = (PlayerPrefs.GetInt(InvertedControlsPref, 0) == 0) ? false : true;
        LeftSensitivity = PlayerPrefs.GetFloat(LeftSensitivityPref, 0.0f);
        RightSensitivity = PlayerPrefs.GetFloat(RightSensitivityPref, 0.0f);

        DontDestroyOnLoad(this);
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(InvertedControlsPref, (InvertedControls) ? 1 : 0);
        PlayerPrefs.SetFloat(LeftSensitivityPref, LeftSensitivity);
        PlayerPrefs.SetFloat(RightSensitivityPref, RightSensitivity);
    }
}
