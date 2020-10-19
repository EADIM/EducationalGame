using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase01_SetValueOnCharacter : MonoBehaviour
{
    public Fase01_References references;
    public TMPro.TMP_InputField inputField;

    public void setValue(int attribute){
        GameObject player = references.Player.gameObject;
        Fase01_PlayerController pcont = player.GetComponent<Fase01_PlayerController>();
        
        if (attribute == 0){
            pcont.setAcceleration(inputField.text);
        }
        else if(attribute == 1){
            pcont.setJumpAngle(inputField.text);
        }
        else if(attribute == 2){
            pcont.setJumpForce(inputField.text);
        }
        else if(attribute == 3){
            pcont.setGravity(inputField.text);
        }
        else if(attribute == 4){
            pcont.setMass(inputField.text);
        }
    }

}
