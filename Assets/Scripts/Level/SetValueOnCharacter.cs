using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValueOnCharacter : MonoBehaviour
{
    public References references;
    public TMPro.TMP_InputField inputField;

    public void setValue(int attribute){
        GameObject player = references.Player.gameObject;
        PlayerController pcont = player.GetComponent<PlayerController>();
        
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
