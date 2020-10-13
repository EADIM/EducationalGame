using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    private static readonly string QualityPref = "Quality";
    public static int Quality_Index = 3;

    private void Awake() 
    {
        Quality_Index = PlayerPrefs.GetInt(QualityPref, 3);

        DontDestroyOnLoad(this);
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(QualityPref, Quality_Index);
    }
}