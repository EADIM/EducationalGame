using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFromAtoB : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject player;

    public Rigidbody player_rb;
    public SimulationMovement player_sm;
    RaycastHit hitInfo;
    Vector3 A_Pos, direction;
    public int MaxRayDistance = 1000;
    private void FixedUpdate() {
        A_Pos = StartPoint.transform.position;
        direction = -Vector3.right * MaxRayDistance;
        if (Physics.Raycast(A_Pos, StartPoint.transform.TransformDirection(direction), out hitInfo, MaxRayDistance)){ 
            if (hitInfo.transform.name == player.transform.name){
                Debug.DrawRay(A_Pos, direction, Color.green);
                player_sm.factor = 0.0000001f;
                player_sm.canMove = false;
                playerJump();
            }
            else{
                Debug.DrawRay(A_Pos, direction, Color.blue);
            }
        }
        else{
            Debug.DrawRay(A_Pos, direction, Color.red);
        }

        player_rb.AddForce(Physics.gravity, ForceMode.Acceleration);
    }

    private void Start() {
        player_rb = player.GetComponent<Rigidbody>();
        player_sm = player.GetComponent<SimulationMovement>();
    }

    private void playerJump(){
        float V0 = player_sm.findV0usingVx(player_sm.HorizontalSpeed, player_sm.JumpAngle);
        float Vx = player_sm.HorizontalSpeed;
        float Vy = player_sm.findVy(V0, player_sm.JumpAngle);
        V0 = Mathf.Ceil(V0);
        Vy = Mathf.Ceil(Vy);
        //Debug.Log("V0: " + V0 + " Vy: " + Vy);
        Vector3 force = new Vector3(0, Vy/10, Vx/10);
        player_rb.AddForce(force * player_rb.mass, ForceMode.Impulse);
    }
}
