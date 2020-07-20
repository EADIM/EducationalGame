using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SimulationMovement : MonoBehaviour
{
    public Vector3 midPosition = new Vector3();

    private Vector3 startPosition;
    public Quaternion startRotation = new Quaternion();
    public bool AllowXMovement = false;
    public bool AllowYMovement = false;
    public bool AllowZMovement = false;

    private int XMovement = 0;
    private int YMovement = 0;
    private int ZMovement = 0;

    public float HorizontalSpeed = 30.0f;
    public float Mass = 50.0f;
    public float gravity = 9.81f;
    public float JumpAngle = 45.0f;
    
    private GameObject thisParent;
    private GameState gameState;
    public GameObject Character;
    public bool canMove = false;
    private Rigidbody rb;

    public float factor = 1.0f;

    private bool reachedMidPlatform = false;
    private bool reachedFinalPlatform = false;

    private bool doOneJump = false;

    private string[] placesCanCollide = {
        "StartPlatform",
        "MidPlatform",
        "FinalPlatform",
        "Wall",
        "Ceiling"
    };

    private void Start() {
        thisParent = this.transform.parent.gameObject;
        gameState = thisParent.GetComponent<GameState>();
        rb = Character.GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
        canMove = gameState.State;
        midPosition = Character.transform.position;
        startPosition = Character.transform.position;
        startRotation = Character.transform.rotation;
        rb.mass = Mass;
    }

    private void Update(){
        canMove = gameState.State;
    }
    private void FixedUpdate() {
        CheckConstraints();

        if(canMove){
            if(gameState.firstStage){
                MoveCharacter(findNextPosition());
            }
            else if (gameState.secondStage){
                if (!doOneJump){
                    playerJump();
                    doOneJump = true;
                }
                
            }
        }
    }

    private Vector3 findNextPosition(){
        Vector3 movementDirection = Vector3.zero;
        movementDirection.z += ZMovement * HorizontalSpeed * Time.deltaTime * factor;

        return movementDirection;
    }

    private void OnCollisionEnter(Collision other) {
        string tag = other.gameObject.tag;
        //Debug.Log("Hit " + tag + "!");
        if(tag == "MidPlatform"){
            rb.velocity = Vector3.zero;
            reachedMidPlatform = true;
            clickPlayButton();
            setPlayButtonText("1/2");
            midPosition = Character.transform.position;
            gameState.firstStage = false;
            gameState.secondStage = true;
            gameState.SwitchState();
        }

        if(tag == "FinalPlatform"){
            rb.velocity = Vector3.zero;
            if (reachedMidPlatform){
                gameState.State = false;
                reachedFinalPlatform = true;
                gameState.winLevel = true;
                setPlayButtonText("2/2");
            }
            else{
                ResetPosition(midPosition);
                doOneJump = false;
                gameState.SwitchState();

            }
        }

        if (tag == "Floor" || tag == "Untagged"){
            if(!reachedMidPlatform || !reachedFinalPlatform){
                ResetValues();
                gameState.SwitchState();
            }
            if(reachedMidPlatform){
                doOneJump = false;
            }
        }
    }

    public void MoveCharacter(Vector3 destination){
        rb.MovePosition(Character.transform.position + destination);
    }

    private void CheckConstraints(){
        XMovement = (AllowXMovement) ? 1 : 0;
        YMovement = (AllowYMovement) ? 1 : 0;
        ZMovement = (AllowZMovement) ? 1 : 0;
    }

    public void ResetValues(){
        setPlayButtonText("PLAY");
        ResetPosition(startPosition);
        ResetConstraints();
        factor = 1.0f;
        reachedMidPlatform = false;
        reachedFinalPlatform = false;
        doOneJump = false;
    }

    public void ResetPosition(Vector3 position){
        Character.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        Character.GetComponent<Rigidbody>().isKinematic = true;
        Character.transform.position = position;
        Character.transform.rotation = startRotation;
        Character.GetComponent<Rigidbody>().isKinematic = false;
        Character.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void ResetConstraints(){
        AllowXMovement = false;
        AllowYMovement = false;
        AllowZMovement = true;
    }

    public void setAngle(string value){
        JumpAngle = ParseValue(value);
    }

    public void setSpeed(string value){
        HorizontalSpeed = ParseValue(value);
    }

    private float ParseValue(string text){
        float value = 0.0f;

        try{
            value = float.Parse(text);
        }
        catch(Exception e){
            Debug.Log("Could not parse " + text);
        }

        return value;
    }

    public float findVy(float V0, float angle){
        float Vy = V0 * Mathf.Sin(Mathf.Deg2Rad * angle);

        return Vy;
    }

    public float findV0usingVx(float Vx, float angle){
        float cosAngle = Mathf.Cos(Mathf.Deg2Rad * angle); 
        
        if (cosAngle == 0.0f){
            return 0.0f;
        }

        float V0 = Vx / cosAngle;

        return V0;
    }

    private GameObject GetChildWithName(GameObject obj, string name) {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            return null;
        }
    }

    private void setPlayButtonText(string text){
        GameObject canvas = GetChildWithName(thisParent.gameObject, "Canvas");
        GameObject buttons = GetChildWithName(canvas.gameObject, "Buttons");
        GameObject playbutton = GetChildWithName(buttons.gameObject, "Play Button");
        GameObject playbutton_text = GetChildWithName(playbutton.gameObject, "Text");
        TMPro.TMP_Text textValue = playbutton_text.GetComponent<TMPro.TMP_Text>();
        textValue.text = text;
    }

    private void clickPlayButton(){
        GameObject canvas = GetChildWithName(thisParent.gameObject, "Canvas");
        GameObject buttons = GetChildWithName(canvas.gameObject, "Buttons");
        GameObject playbutton = GetChildWithName(buttons.gameObject, "Play Button");
        playbutton.GetComponent<KeepButtonColor>().onButtonClick();
    }

    private void playerJump(){
        float V0 = findV0usingVx(HorizontalSpeed, JumpAngle);
        float Vx = HorizontalSpeed;
        float Vy = findVy(V0, JumpAngle);
        V0 = Mathf.Ceil(V0);
        Vy = Mathf.Ceil(Vy);
        //Debug.Log("V0: " + V0 + " Vy: " + Vy);
        Vector3 force = new Vector3(0, Vy/10, Vx/10);
        rb.AddForce(force * rb.mass, ForceMode.Impulse);
    }

}
