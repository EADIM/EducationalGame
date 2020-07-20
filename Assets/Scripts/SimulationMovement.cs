using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SimulationMovement : MonoBehaviour
{
    public Vector3 startPosition = new Vector3();
    public Quaternion startRotation = new Quaternion();
    public bool AllowXMovement = false;
    public bool AllowYMovement = false;
    public bool AllowZMovement = false;

    private int XMovement = 0;
    private int YMovement = 0;
    private int ZMovement = 0;

    public float Speed = 30.0f;
    public float Mass = 50.0f;
    public float gravity = 9.81f;
    public float JumpAngle = 45.0f;
    
    private GameObject thisParent;
    private GameState gameState;
    public GameObject Character;
    public bool canMove = false;
    private Rigidbody rb;

    public float factor = 1.0f;

    private void Start() {
        thisParent = this.transform.parent.gameObject;
        gameState = thisParent.GetComponent<GameState>();
        rb = Character.GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
        canMove = gameState.State;
        startPosition = Character.transform.position;
        startRotation = Character.transform.rotation;
    }

    private void Update(){
        canMove = gameState.State;
    }
    private void FixedUpdate() {
        CheckConstraints();

        if(canMove){
            MoveCharacter(findNextPosition());
        }
    }

    private Vector3 findNextPosition(){
        Vector3 movementDirection = Vector3.zero;
        movementDirection.z += ZMovement * Speed * Time.deltaTime * factor;

        return movementDirection;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "MidPlatform"){
            rb.velocity = Vector3.zero;
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
        ResetPosition();
        ResetConstraints();
        factor = 1.0f;
    }

    public void ResetPosition(){
        Character.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        Character.GetComponent<Rigidbody>().isKinematic = true;
        Character.transform.position = startPosition;
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
        Speed = ParseValue(value);
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
        float V0 = Vx / Mathf.Cos(Mathf.Deg2Rad * angle);

        return V0;
    }
}
