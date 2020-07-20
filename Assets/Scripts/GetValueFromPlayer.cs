using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetValueFromPlayer : MonoBehaviour
{
    public GameObject player;
    private SimulationMovement sim;
    public int attribute = 0;

    private void Start() {
        sim = player.GetComponent<SimulationMovement>();
    }

    private void Update() {
        setText(getAttribute());
    }

    private float getAttribute(){
        float value = 0.0f;
        if (attribute == 0){
            value = sim.HorizontalSpeed;
        }
        else if (attribute == 1){
            value = sim.JumpAngle;
        }
        else if (attribute == 2){
            value = sim.Mass;
        }
        else if (attribute == 3){
            value = sim.gravity;
        }

        return value;
    }

    private void setText(float value){
        this.GetComponent<TMPro.TMP_Text>().text = value.ToString();
    }
}
