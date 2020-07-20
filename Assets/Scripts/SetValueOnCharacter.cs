using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValueOnCharacter : MonoBehaviour
{
    public TMPro.TMP_InputField inputField;
    public GameObject Object;

    public void setValue(int attribute){
        SimulationMovement sim = Object.GetComponent<SimulationMovement>();
        
        if (attribute == 0){
            sim.setSpeed(inputField.text);
        }
        else if(attribute == 1){
            sim.setAngle(inputField.text);
        }
    }

}
