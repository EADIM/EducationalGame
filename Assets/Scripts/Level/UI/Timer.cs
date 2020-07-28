using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
     private float seconds = 0.0f;
    private int minutes = 0;
    private int hours = 0;
    void Update()
    {
        seconds += Time.deltaTime;
        if(seconds + Time.deltaTime >= 60){
            minutes += 1;
            seconds = 0;
        }
        if(minutes > 60){
            hours += 1;
            minutes = 0;
        }

        GetComponent<TMPro.TMP_Text>().text = hours.ToString("0#") + ":" + minutes.ToString("0#") + ":" + seconds.ToString("0#.");
    }

    public void Reset(){
        seconds = 0.0f;
        minutes = 0;
        hours = 0;

    }
}
