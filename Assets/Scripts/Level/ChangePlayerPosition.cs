using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPosition : MonoBehaviour
{
    public GameObject player;
    public GameObject gameStateObject;

    SimulationMovement sm;
    GameState gms;

    private void Start() {
        sm = player.GetComponent<SimulationMovement>();
        gms = gameStateObject.GetComponent<GameState>();
    }

    private void OnMouseDown() {
        if(gms.States["Exploration"]){
            Debug.Log("Clicou no collider do " + transform.parent.name);
            Debug.Log("Posição do Coliider: " + transform.position);
            if(sm.checkpoints.Count > 0){
                Vector3 newPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                player.GetComponent<SimulationMovement>().checkpoints[0].setPosition(newPosition);
            }
            sm.ResetPosition(sm.checkpoints[0]);
        }
    }
}
