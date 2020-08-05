using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    public Camera Camera01;
    public Camera Camera02;

    public bool isMain = true;
    private bool isSwitch = false;


    private void Start() {
        if(isMain){
            Camera01.enabled = true;
            Camera02.enabled = false;
        }
        else{
            Camera01.enabled = false;
            Camera02.enabled = false;
        }
    }
    
    public void switchCameras(){
        Camera01.enabled = isSwitch;
        Camera02.enabled = !isSwitch;
        isSwitch = !isSwitch;
    }

    public void switchToCamera01(){
        Camera01.enabled = true;
        Camera02.enabled = false;
    }

    public void switchToCamera02(){
        Camera01.enabled = false;
        Camera02.enabled = true;
    }
}
