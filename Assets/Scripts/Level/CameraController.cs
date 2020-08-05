using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float camMovSpeed = 100.0f;
    public float camRotSpeed = 100.0f;
    public float verticalSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float sensitivity = 0.2f;
    public float rotationFactor = 1.0f;

    public References references;

    private GameObject ExplorerCamera;
    private GameObject XAxisPivot;
    private Joystick leftJoystick, rightJoystick;
    private BoxCollider mapBoundaries;
    public Vector3 InitialPosition;
    public Quaternion InitialRotation;

    public static bool AllowMovement = true;

    [SerializeField]
    private bool debugMovement = false;

    private void Start() {
        ExplorerCamera = references.ExplorerCamera;
        GameObject JoystickContainer = Utils.GetChildWithName(references.Canvas.gameObject, "Joysticks Container");
        leftJoystick = Utils.GetChildWithName(JoystickContainer, "Left Joystick").GetComponent<FixedJoystick>();
        rightJoystick = Utils.GetChildWithName(JoystickContainer, "Right Joystick").GetComponent<FixedJoystick>();
        mapBoundaries = references.Boundaries.GetComponent<BoxCollider>();
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
    }

    void Update()
    {
        if (AllowMovement){
            getMovementInput();
            getRotationInput();
        }
    }

    private void getMovementInput()
    {

        float joyHInput = leftJoystick.Horizontal;
        float joyVInput = leftJoystick.Vertical;

        if(debugMovement)
        {
            Debug.LogFormat("Left_joystickH: {0}   Left_joystickV: {1}", joyHInput, joyVInput);
        }

        float keyboardHInput = Input.GetAxis("Horizontal");
        float keyboardVInput = Input.GetAxis("Vertical");

        float hMov = Mathf.Clamp(keyboardHInput + joyHInput, -1.0f, 1.0f) * Time.deltaTime * camMovSpeed;
        float vMov = Mathf.Clamp(keyboardVInput + joyVInput, -1.0f, 1.0f) * Time.deltaTime * camMovSpeed;
        float upMov = 0.0f;

        if(Input.GetKey(KeyCode.Space))
        {
            upMov += verticalSpeed;
        }

        if(Input.GetKey(KeyCode.LeftAlt))
        {
            upMov -= verticalSpeed;
        }

        upMov *= Time.deltaTime * camMovSpeed;

        Vector3 RIGHT = transform.TransformDirection(Vector3.right);
        Vector3 FORWARD = transform.TransformDirection(Vector3.forward);
        Vector3 UP = transform.TransformDirection(Vector3.up);

        Vector3 pos = transform.position;
        pos += FORWARD * vMov; // Z axis
        pos += RIGHT * hMov; // X axis
        pos += UP * upMov; // Y axis

        if(mapBoundaries.enabled){
            pos.x = Mathf.Clamp(pos.x, mapBoundaries.bounds.min.x,mapBoundaries.bounds.max.x);
            pos.y = Mathf.Clamp(pos.y, mapBoundaries.bounds.min.y,mapBoundaries.bounds.max.y);
            pos.z = Mathf.Clamp(pos.z, mapBoundaries.bounds.min.z,mapBoundaries.bounds.max.z);
        }

        //Debug.Log("Clamped position = " + pos.ToString());

        transform.position = pos;
    }

    private void getRotationInput()
    {
        float hRot = 0.0f;
        float vRot = 0.0f;

        float joyHInput = rightJoystick.Horizontal;
        float joyVInput = rightJoystick.Vertical;

        if(debugMovement)
        {
            Debug.LogFormat("Right_joystickH: {0}   Right_joystickV: {1}", joyHInput, joyVInput);
        }

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

        if (joyHInput >= sensitivity){
            hRot += rotationSpeed;
        }
        else if (joyHInput <= -sensitivity){
            hRot -= rotationSpeed;    
        }
        if (joyVInput >= sensitivity){
            vRot -= rotationSpeed;
        }
        else if (joyVInput <= -sensitivity){
            vRot += rotationSpeed;
        }
        
        hRot *= Time.deltaTime * camRotSpeed;
        vRot *= Time.deltaTime * camRotSpeed;

        Vector3 rot = new Vector3(-vRot, hRot, 0.0f);
        transform.Rotate(rot, Space.World);
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }

    public void switchMovement()
    {
        AllowMovement = !AllowMovement;
    }
}
