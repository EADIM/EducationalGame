using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float camMovSpeed = 100.0f;
    public float camRotSpeed = 100.0f;
    public float verticalSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float sensitivity = 0.2f;

    public GameObject gameObj;
    public Joystick leftJoystick, rightJoystick;

    public bool AllowMovement = true;

    void Update()
    {
        if (AllowMovement){
            getMovementInput();
            getRotationInput();
        }
    }

    private void getMovementInput(){

        float joyHInput = leftJoystick.Horizontal;
        float joyVInput = leftJoystick.Vertical;

        float keyboardHInput = Input.GetAxis("Horizontal");
        float keyboardVInput = Input.GetAxis("Vertical");

        float hMov = (keyboardHInput + joyHInput) * Time.deltaTime * camMovSpeed;
        float vMov = (keyboardVInput + joyVInput) * Time.deltaTime * camMovSpeed;
        float upMov = 0.0f;

        //printStuff(joyHInput, joyVInput, "joyHInput", "joyVInput");
        //printStuff(keyboardHInput, keyboardVInput, "keyBoardH", "keyBoardV");

        if(Input.GetKey(KeyCode.Space)){
            upMov += verticalSpeed;
        }

        if(Input.GetKey(KeyCode.LeftAlt)){
            upMov -= verticalSpeed;
        }

        upMov *= Time.deltaTime * camMovSpeed;

        Vector3 pos = new Vector3(hMov, upMov, vMov);
        gameObj.transform.Translate(pos);
    }

    private void getRotationInput(){
        float hRot = 0.0f;
        float vRot = 0.0f;

        if (Input.GetKey(KeyCode.J)){
            hRot -= rotationSpeed;
        }

        if (Input.GetKey(KeyCode.L)){
            hRot += rotationSpeed;
        }

        if (Input.GetKey(KeyCode.I)){
            vRot -= rotationSpeed;
        }

        if (Input.GetKey(KeyCode.K)){
            vRot += rotationSpeed;
        }

        if (rightJoystick.Horizontal >= sensitivity){
            hRot += rotationSpeed;
        }
        else if (rightJoystick.Horizontal <= -sensitivity){
            hRot -= rotationSpeed;    
        }
        if (rightJoystick.Vertical >= sensitivity){
            vRot -= rotationSpeed;
        }
        else if (rightJoystick.Vertical <= -sensitivity){
            vRot += rotationSpeed;
        }
        
        hRot *= Time.deltaTime * camRotSpeed;
        vRot *= Time.deltaTime * camRotSpeed;

        Vector3 rot = new Vector3(vRot, hRot, 0.0f);
        gameObj.transform.Rotate(rot);
    }

    private void printStuff(float hMov, float vMov, string hstr, string vstr){
        Debug.Log(
            "\n" + hstr + ": " + hMov + 
            " || " + vstr + ": " + vMov + "\n");
    }

    public void switchMovement(){
        AllowMovement = !AllowMovement;
    }
}
