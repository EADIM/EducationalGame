using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetValueFromPlayer : MonoBehaviour
{
    public References references;

    private PlayerController sim;
    public int attribute = 0;

    private void Start() {
        sim = references.Player.GetComponent<PlayerController>();
    }

    private void Update() {
        setText(getAttribute());
    }

    private float getAttribute(){
        float value = 0.0f;
        if (attribute == 0){
            value = sim.Acceleration;
        }
        else if (attribute == 1){
            value = sim.JumpAngle;
        }
        else if (attribute == 2){
            value = sim.Mass;
        }
        else if (attribute == 3){
            value = Mathf.Abs(sim.Gravity);
        }
        else if (attribute == 4){
            value = sim.JumpForce;
        }

        return value;
    }

    private void setText(float value){
        this.GetComponent<TMPro.TMP_Text>().text = value.ToString();
    }
}
