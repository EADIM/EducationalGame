using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFromAtoB : MonoBehaviour
{

    public References references;
    public int MaxRayDistance = 1000;
    public bool debug = false;

    private GameObject StartPoint;
    private Rigidbody playerRigidbody;
    private PlayerController playerController;
    
    RaycastHit hitInfo;
    Vector3 A_Pos, direction;

    private void Start() 
    {
        playerRigidbody = references.Player.GetComponent<Rigidbody>();
        playerController = references.Player.GetComponent<PlayerController>();
        StartPoint = references.RaycastCenter;
    }

    private void FixedUpdate()
    {
        A_Pos = StartPoint.transform.position;
        direction = Vector3.right * MaxRayDistance;

        if(transform.gameObject.GetComponent<GameState>().States[transform.gameObject.GetComponent<GameState>().getSimulationName()] || debug)
        {
            if (Physics.Raycast(A_Pos, StartPoint.transform.TransformDirection(direction), out hitInfo, MaxRayDistance))
            {
                if (hitInfo.transform.tag == "Player")
                {
                    Debug.DrawRay(A_Pos, StartPoint.transform.TransformDirection(direction), Color.green);
                    playerController.IsPlayerOnInitialPlatform = false;
                    playerController.Jump();
                }
                else
                {
                    Debug.DrawRay(A_Pos, StartPoint.transform.TransformDirection(direction), Color.blue);
                }
            }
        }
    }
}
