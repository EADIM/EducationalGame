using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    
    public Camera exploreCamera;
    public Camera playerCamera;

    private bool isSwitch = false;

    private void Start() {
        playerCamera.enabled = isSwitch;
    }
    
    public void switchCamera(){
        exploreCamera.enabled = isSwitch;
        playerCamera.enabled = !isSwitch;
        isSwitch = !isSwitch;
    }
}
