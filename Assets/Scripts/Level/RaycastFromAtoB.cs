using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFromAtoB : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject player;

    private Rigidbody player_rb;
    private SimulationMovement player_sm;
    RaycastHit hitInfo;
    Vector3 A_Pos, direction;
    public int MaxRayDistance = 1000;

    public bool debug = false;

    private void FixedUpdate() {
        A_Pos = StartPoint.transform.position;
        direction = Vector3.right * MaxRayDistance;
        if(transform.gameObject.GetComponent<GameState>().States[transform.gameObject.GetComponent<GameState>().getSimulationName()] || debug){
            if (Physics.Raycast(A_Pos, StartPoint.transform.TransformDirection(direction), out hitInfo, MaxRayDistance)){

                if (hitInfo.transform.tag == "Player"){
                    Debug.DrawRay(A_Pos, StartPoint.transform.TransformDirection(direction), Color.green);
                    
                    player_sm.Jump(true);
                    player_sm.canMove = false;
                    player_sm.didJumpMid = true;
                    player_sm.dontAccelerate = true;

                    if (player_sm.factor > 0){
                        player_sm.zeroFactor();
                    }
                }
                else{
                    Debug.DrawRay(A_Pos, StartPoint.transform.TransformDirection(direction), Color.blue);
                }
            }
        }
    }

    private void Start() {
        player_rb = player.GetComponent<Rigidbody>();
        player_sm = player.GetComponent<SimulationMovement>();
    }
}
