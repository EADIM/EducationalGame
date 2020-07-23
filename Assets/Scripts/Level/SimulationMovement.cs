using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SimulationMovement : MonoBehaviour
{
    public bool AllowXMovement = false;
    public bool AllowYMovement = false;
    public bool AllowZMovement = false;

    private int XMovement = 0;
    private int YMovement = 0;
    private int ZMovement = 0;

    public float acceleration = 0.0f;
    public float Mass = 100.0f;
    public float gravity = 9.81f;
    public float jumpAngle = 0.0f;
    public float factor = 1.0f; 
    public bool canMove = false;   

    private GameObject thisParent;
    private GameState gameState;
    private Rigidbody rb;

    private Dictionary<string, bool> CollidablePlaces = new Dictionary<string, bool>();

    public List<Checkpoint> checkpoints = new List<Checkpoint>();

    public bool didJumpMid = false;
    public bool didJumpFinal = false;

    public bool moved = false;

    public Vector3 currentPosition;
    public Vector3 previousPosition;

    public bool ischecking = false;

    private void Awake() {
        InitializeCollidablePlaces();
        checkpoints.Add(new Checkpoint(transform.position, transform.rotation, factor));
    }

    private void Start() {
        gameState = transform.parent.GetComponent<GameState>();
        rb = GetComponent<Rigidbody>();
        rb.mass = Mass;
        canMove = gameState.States["Simulation"];
        currentPosition = transform.position;
        previousPosition = transform.position;
    }

    private void Update(){
        canMove = gameState.States["Simulation"];
    }
    
    private void FixedUpdate() {
        CheckConstraints();

        if(canMove){
            MoveCharacter();
        }

        previousPosition = currentPosition;
        currentPosition = transform.position;

        rb.AddForce(new Vector3 (0, -gravity * rb.mass, 0), ForceMode.Acceleration);
    }

    public void MoveCharacter(){
        Vector3 force = new Vector3(0.0f, 0.0f, 1.0f);
        rb.AddForce(force * acceleration, ForceMode.Acceleration);  
    }

    private void OnCollisionEnter(Collision other) {
        string tag = other.gameObject.tag;

        if (!CollidablePlaces[tag]){
            //Collision with forbidden object
            //Debug.Log("PROIBIDO: Colisão com " + tag);
            gameState.SwitchState("Lost");

        }
        else{
            //Collission with allowed object
            //Debug.Log("PERMITIDO: Colisão com " + tag);
            if (tag == "MidPlatform"){
            }
        }
    }

    private void OnCollisionStay(Collision other) {
        string tag = other.gameObject.tag;

        if (!CollidablePlaces[tag]){
            //Collision with forbidden object
            //Debug.Log("PROIBIDO: Colisão com " + tag);
            gameState.SwitchState("Lost");

        }
        else{
            //Collission with allowed object
            //Debug.Log("PERMITIDO: Colisão com " + tag);
            if (tag == "StartPlatform"){
                if(checkpoints.Count == 0){
                    checkpoints.Add(new Checkpoint(transform.position, transform.rotation, factor, false, false));
                    gameState.playMessage(1);
                }
                if (checkIfObjectIsStill(this.gameObject) && (didJumpMid || moved)){
                    Reset(checkpoints[checkpoints.Count - 1]);
                    gameState.SwitchState("Exploration");
                    gameState.playMessage(0);
                }
            }

            if (tag == "MidPlatform"){
                if(checkIfObjectIsStill(this.gameObject) && checkpoints.Count == 1){
                    checkpoints.Add(new Checkpoint(transform.position, checkpoints[0].getRotation(), 0.0f, true, false));
                    gameState.playMessage(1);
                    Reset(checkpoints[checkpoints.Count - 1]);
                    gameState.SwitchState("Exploration");
                }

                if(checkpoints.Count == 2){
                    if (!didJumpFinal){ //Se ele ainda não tiver pulado
                        if(gameState.States["Simulation"]){
                            Jump();
                            didJumpFinal = true;
                        }
                    }
                    else{ //Se ele tiver pulado
                        if (checkIfObjectIsStill(this.gameObject)){
                            Reset(checkpoints[checkpoints.Count - 1]);
                            gameState.SwitchState("Exploration");
                            gameState.playMessage(0);
                        }
                    }
                }

            }

            if (tag == "FinalPlatform"){
                if (checkIfObjectIsStill(this.gameObject) && checkpoints.Count == 2){    
                    checkpoints.Add(new Checkpoint(transform.position, checkpoints[0].getRotation(), 0.0f, true, true));
                    gameState.playMessage(1);
                    gameState.SwitchState("Win");
                }
            }
        }
    }

    private void CheckConstraints(){
        XMovement = (AllowXMovement) ? 1 : 0;
        YMovement = (AllowYMovement) ? 1 : 0;
        ZMovement = (AllowZMovement) ? 1 : 0;
    }

    public void Reset(){
        Checkpoint lastCkp = checkpoints[checkpoints.Count - 1];
        ResetValues(lastCkp);
        ResetPosition(lastCkp);
        ResetConstraints();
    }

    public void Reset(Checkpoint ckp){
        ResetValues(ckp);
        ResetPosition(ckp);
        ResetConstraints();
    }

    public void ResetValues(Checkpoint ckp){
        factor = ckp.getFactor();
        didJumpMid = ckp.getJumpMid();
        didJumpFinal = ckp.getJumpFinal();
        moved = false;
    }

    public void ResetPosition(Checkpoint checkpoint){
        Debug.Log("Reset Position");

        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = checkpoint.getPosition();
        transform.rotation = checkpoint.getRotation();
        GetComponent<Rigidbody>().isKinematic = false;

        if (checkpoints.Count == 2){
            didJumpFinal = false;
        }
    }

    public void ResetConstraints(){
        AllowXMovement = false;
        AllowYMovement = false;
        AllowZMovement = true;
    }

    public void ResetEverythingFromScratch(){
        Reset(checkpoints[0]);
        checkpoints.Clear();
    }

    public void setAngle(string value){
        jumpAngle = ParseValue(value);
    }

    public void setAcceleration(string value){
        acceleration = ParseValue(value);
    }

    
    private bool checkIfObjectIsStill(GameObject cube){
        if(currentPosition.x != previousPosition.x ||
            currentPosition.y != previousPosition.y ||
            currentPosition.z != previousPosition.z)
        {
            //Debug.Log("The cube is kinda still, so I see that as an absolute win.");
            return false;
        }

        return true;
    }
    
    /*
    public bool checkIfObjectIsStill(GameObject obj){

        bool ObjIsStill = false;

        StartCoroutine(
            CheckMoving(
                obj, //gameobject
                (i)=> { ObjIsStill = i; } //callback function
            )
        );

        return ObjIsStill;
    }

    private IEnumerator CheckMoving(GameObject gmobj, System.Action<bool> callback)
    {
        Debug.Log("COROTINA!");
        Vector3 startPos = gmobj.transform.position;

        yield return new WaitForSeconds(0.4f);

        Vector3 finalPos = gmobj.transform.position;

        if( startPos.x != finalPos.x ||
            startPos.y != finalPos.y ||
            startPos.z != finalPos.z)
            {
                callback(true);
            }
        
        yield return 0;
    }
    */

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

    public void Jump(){
        float Vx = 0.0f;
        float Vz = rb.velocity.z;
        float V0 = findV0UsingVx(Vz, jumpAngle);
        float Vy = findVyUsingV0(V0, jumpAngle);

        V0 = Mathf.Ceil(V0);
        Vy = Mathf.Ceil(Vy);

        Vector3 force = new Vector3(Vx, Vy, Vz);
        Debug.Log("force = ("+ force.x + ", "+ force.y + ", " + force.z + ")");

        //force *= force.magnitude;
        Debug.Log("force * magnitude = ("+ force.x + ", "+ force.y + ", " + force.z + ")");

        rb.AddForce(force,  ForceMode.Impulse);
    }

    public float findVyUsingV0(float V0, float angle){
        float Vy = V0 * Mathf.Sin(Mathf.Deg2Rad * angle);

        return Vy;
    }

    public float findV0UsingVx(float Vx, float angle){
        float cosAngle = Mathf.Cos(Mathf.Deg2Rad * angle); 
        
        if (cosAngle == 0.0f){
            return 0.0f;
        }

        float V0 = Vx / cosAngle;

        return V0;
    }

    public void zeroFactor(){
        StartCoroutine(
            CoroutineFactor(
                (i)=>{ factor = i; },  //callback
                0.02f  //waitTime
            )
        );
    }

    public IEnumerator CoroutineFactor(System.Action<float> callback, float waitTime){
        yield return new WaitForSeconds(1 * waitTime);
        callback(0.0f);
        //print("ZERO FACTOR!");
    }

    private void InitializeCollidablePlaces(){
        CollidablePlaces.Add("Untagged", false);
        CollidablePlaces.Add("Floor", false);
        CollidablePlaces.Add("Ceiling", true);
        CollidablePlaces.Add("Wall", true);
        CollidablePlaces.Add("StartPlatform", true);
        CollidablePlaces.Add("MidPlatform", true);
        CollidablePlaces.Add("FinalPlatform", true);
    }

}
