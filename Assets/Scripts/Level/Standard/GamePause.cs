using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    [SerializeField]
    private static bool isPaused = false;

    public static void PauseGame(){
        //Debug.Log("Time.timeScale = 0");
        Time.timeScale = 0;
        isPaused = true;
    }

    public static void ResumeGame(){
        //Debug.Log("Time.timeScale = 1");
        Time.timeScale = 1;
        isPaused = false;
    }
}
