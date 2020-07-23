using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    public void PauseGame(){
        //Debug.Log("Time.timeScale = 0");
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        //Debug.Log("Time.timeScale = 1");
        Time.timeScale = 1;
    }
}
