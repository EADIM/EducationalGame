using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPosition : MonoBehaviour
{
    public GameObject player;
    public GameObject gameStateObject;

    SimulationMovement sm;
    GameState gms;
    GetProblemInfo gpi;

    private void Start() {
        sm = player.GetComponent<SimulationMovement>();
        gms = gameStateObject.GetComponent<GameState>();
        gpi = gameStateObject.GetComponent<GetProblemInfo>();
    }

    private void OnMouseDown() {
        if(gms.States[gms.getExplorationName()]){
            Debug.Log("Clicou no collider do " + transform.parent.name);
            Debug.Log("Posição do Coliider: " + transform.position);
            if(sm.checkpoints.Count > 0){
                Vector3 newPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                player.GetComponent<SimulationMovement>().checkpoints[0].setPosition(newPosition);
                gpi.ColisorPlataformaInicial = transform.GetComponent<BoxCollider>();
                gpi.OnIntialPlatformChange();
            }
            sm.ResetPosition(sm.checkpoints[0]);
        }
    }
}
