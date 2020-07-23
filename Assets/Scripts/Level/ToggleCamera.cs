using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    
    public Camera exploreCamera;
    public Camera playerCamera;

    private bool isSwitch = false;

    private void Start() {
        exploreCamera.enabled = true;
        playerCamera.enabled = false;
    }
    
    public void switchCamera(){
        exploreCamera.enabled = isSwitch;
        playerCamera.enabled = !isSwitch;
        isSwitch = !isSwitch;
    }

    public void switchToExplore(){
        exploreCamera.enabled = true;
        playerCamera.enabled = false;
    }

    public void switchToPlayer(){
        exploreCamera.enabled = false;
        playerCamera.enabled = true;
    }
}
