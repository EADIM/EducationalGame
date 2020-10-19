using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase01_RaycastFromAtoB : MonoBehaviour
{

    public Fase01_References references;
    public int MaxRayDistance = 1000;
    public bool debug = false;

    public GameObject StartPoint;
    private Rigidbody playerRigidbody;
    private Fase01_PlayerController playerController;
    
    RaycastHit hitInfo;
    Vector3 A_Pos, direction;

    private void Start() 
    {
        playerRigidbody = references.Player.GetComponent<Rigidbody>();
        playerController = references.Player.GetComponent<Fase01_PlayerController>();
        StartPoint = references.RaycastCenter;
    }

    private void FixedUpdate()
    {
        A_Pos = StartPoint.transform.position;
        direction = Vector3.right * MaxRayDistance;

        if(transform.gameObject.GetComponent<Fase01_GameState>().States[transform.gameObject.GetComponent<Fase01_GameState>().getSimulationName()] || debug)
        {
            if (Physics.Raycast(A_Pos, StartPoint.transform.TransformDirection(direction), out hitInfo, MaxRayDistance))
            {
                if (hitInfo.transform.tag == "Player" || hitInfo.transform.name == "Player")
                {
                    Debug.Log("Hit player");
                    Debug.DrawRay(A_Pos, StartPoint.transform.TransformDirection(direction), Color.green);
                    playerController.IsPlayerOnInitialPlatform = false;
                    playerController.Jump(playerController.GetJumpVector(), ForceMode.VelocityChange);
                }
                else
                {
                    Debug.DrawRay(A_Pos, StartPoint.transform.TransformDirection(direction), Color.blue);
                }
            }
        }
    }

    public void setStartPoint(GameObject newPoint){
        StartPoint = newPoint;
        //Debug.LogFormat("StartPoint Position: {0}", StartPoint.transform.position.ToString());
    }
}
