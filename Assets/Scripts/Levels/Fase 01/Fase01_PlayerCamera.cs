using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class Fase01_PlayerCamera : MonoBehaviour
{
    public Fase01_References references;
    public Vector3 distance;
    public bool BackCam = true;
    
    private GameObject player;
    

    private void Start() {
        player = references.Player;
    }

    void Update()
    {
        if(BackCam){
            transform.SetPositionAndRotation(getBackCamPosition(), transform.rotation);
        }
        else{
            transform.SetPositionAndRotation(getProfileCamPosition(), transform.rotation);
        }
    }

    private Vector3 getBackCamPosition(){
        return new Vector3(
                player.transform.position.x + distance.x,
                player.transform.position.y + distance.y,
                player.transform.position.z + distance.z
            );
    }

    private Vector3 getProfileCamPosition(){
        return new Vector3(
                transform.position.x,
                transform.position.y,
                player.transform.position.z
            );
    }
}
