using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public GameObject player;
    
    private SimulationMovement simulation;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        simulation = player.GetComponent<SimulationMovement>();
        rend = player.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(transform.name + " entrou em contato com " + other.transform.name);
        if(other.transform.name == player.transform.name){
            if(transform.parent.tag == "MidPlatform"){
                simulation.stopAllMovement();
                rend.material.SetColor("PlaneBase", Color.green);
            }
            else{
                if(simulation.checkpoints.Count == 2){
                    simulation.stopAllMovement();
                }
            }
        }
    }
}
