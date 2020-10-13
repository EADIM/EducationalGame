using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    private int camera_index = 1;
    public Camera[] cameras = new Camera[3];
    [HideInInspector] public Camera SideCamera;
    [HideInInspector] public Camera ExplorerCamera;
    [HideInInspector] public Camera BackCamera;

    public Camera currentCamera;
    public Camera previousCamera;
    private string currentActiveCamera = "Explorer Camera";
    private string previousActiveCamera = "None";

    private void Awake() {
        //Debug.Log("Length of Cameras: " + cameras.Length);
        SideCamera = cameras[0];
        ExplorerCamera = cameras[1];
        BackCamera = cameras[2];

        currentCamera = ExplorerCamera;
    }

    private void Start() {
        SideCamera.enabled = false;
        ExplorerCamera.enabled = true;
        BackCamera.enabled = false;
    }
    
    public void switchCameras(){
        for (int i = 0; i < cameras.Length; i++){
            if (i == camera_index){
                cameras[i].enabled = true;
            }
            else{
                cameras[i].enabled = false;
            }
        }

        previousCamera = currentCamera;
        previousActiveCamera = previousCamera.name;
        currentCamera = cameras[camera_index];
        currentActiveCamera = currentCamera.name;
        
        camera_index++;

        camera_index = (camera_index >= cameras.Length) ? 0 : camera_index;

        Debug.Log("Selected " + currentActiveCamera);
    }

    public void switchToCamera(Camera current, Camera destiny){
        current.enabled = false;
        destiny.enabled = true;

        currentCamera = destiny;
        currentActiveCamera = destiny.name;
    }

    public string getActiveCameraName(){
        return currentActiveCamera;
    }
}
