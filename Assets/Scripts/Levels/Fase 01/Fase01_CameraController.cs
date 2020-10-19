using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase01_CameraController : MonoBehaviour
{
    public float camMovSpeed = 100.0f;
    public float camRotSpeed = 100.0f;
    public float verticalSpeed = 1.0f;
    public float PositionSensitivity = 0.0f;
    public float RotationSensitivity = 0.0f;
    public float rotationFactor = 1.0f;

    public Fase01_References references;

    private GameObject ExplorerCamera;
    private GameObject XAxisPivot;
    private Joystick leftJoystick, rightJoystick;
    private BoxCollider mapBoundaries;
    public Vector3 InitialPosition;
    public Quaternion InitialRotation;
    [SerializeField] public static bool AllowMovement = true;
    [SerializeField] private bool debugMovement = false;
    public static bool isLMoving = false;
    public static bool isRMoving = false;

    private ToggleCamera toggle;


    private void Start() {
        toggle = references.GameState.GetComponent<ToggleCamera>();
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
        if (AllowMovement && toggle.currentCamera.name == transform.name){
            getMovementInput();
            getRotationInput();
        }
    }

    private void getMovementInput()
    {

        float joyHInput = -leftJoystick.Horizontal;
        float joyVInput = -leftJoystick.Vertical;
        float keyboardHInput = -Input.GetAxis("Horizontal");
        float keyboardVInput = -Input.GetAxis("Vertical");

        float hMagnitude = Mathf.Abs(joyHInput);
        float vMagnitude = Mathf.Abs(joyVInput);

        if (hMagnitude < ControlsManager.LeftSensitivity)
        {
            joyHInput = 0.0f;
        }

        if (vMagnitude < ControlsManager.LeftSensitivity)
        {
            joyVInput = 0.0f;
        }

        if(debugMovement)
        {
            Debug.LogFormat("Left_joystickH: {0}   Left_joystickV: {1}", joyHInput, joyVInput);
        }

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

        int DIRECTION = (ControlsManager.InvertedControls) ? 1 : -1;
        Vector3 pos = transform.position;
        pos += DIRECTION * FORWARD * vMov; // Z axis
        pos += DIRECTION * RIGHT * hMov; // X axis
        pos += DIRECTION * UP * upMov; // Y axis

        if(mapBoundaries.enabled){
            pos.x = Mathf.Clamp(pos.x, mapBoundaries.bounds.min.x,mapBoundaries.bounds.max.x);
            pos.y = Mathf.Clamp(pos.y, mapBoundaries.bounds.min.y,mapBoundaries.bounds.max.y);
            pos.z = Mathf.Clamp(pos.z, mapBoundaries.bounds.min.z,mapBoundaries.bounds.max.z);
        }

        //Debug.Log("Clamped position = " + pos.ToString());

        transform.position = pos;

        if(hMagnitude == 0.0f && vMagnitude == 0.0f)
        {
            isLMoving = false;
        }
        else
        {
            isLMoving = true;
        }
    }

    private void getRotationInput()
    {
        float hRot = 0.0f;
        float vRot = 0.0f;

        float joyHInput = -rightJoystick.Horizontal;
        float joyVInput = -rightJoystick.Vertical;
        float KeyboardHInput = 0.0f;
        float KeyboardVInput = 0.0f;

        if (Input.GetKey(KeyCode.J)) // left
        {
            KeyboardHInput = -1;
        }

        if (Input.GetKey(KeyCode.L)) // right
        {
            KeyboardHInput = 1;
        }

        if (Input.GetKey(KeyCode.I))  // up
        {
            KeyboardVInput = 1;
        }

        if (Input.GetKey(KeyCode.K)) // down
        {
            KeyboardVInput = -1;
        }

        float hMagnitude = Mathf.Abs(joyHInput);
        float vMagnitude = Mathf.Abs(joyVInput);

        if (hMagnitude < ControlsManager.RightSensitivity)
        {
            joyHInput = 0.0f;
        }

        if (vMagnitude < ControlsManager.RightSensitivity)
        {
            joyVInput = 0.0f;
        }

        if(debugMovement)
        {
            Debug.LogFormat("Right_joystickH: {0}   Right_joystickV: {1}", joyHInput, joyVInput);
        }
        
        hRot = Mathf.Clamp(KeyboardHInput + joyHInput, -1.0f, 1.0f) * Time.deltaTime * camRotSpeed;
        vRot = Mathf.Clamp(KeyboardVInput + joyVInput, -1.0f, 1.0f) * Time.deltaTime * camRotSpeed;

        int DIRECTION = (ControlsManager.InvertedControls) ? 1 : -1;
        Vector3 rot = new Vector3(vRot, hRot, 0.0f);
        rot *= DIRECTION;
        transform.Rotate(rot, Space.World);
        
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

        if(hMagnitude == 0.0f && vMagnitude == 0.0f)
        {
            isRMoving = false;
        }
        else
        {
            isRMoving = true;
        }
    }
}
