using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SimulationMovement : MonoBehaviour
{
    public float acceleration = 0.0f;
    public float Mass = 100.0f;
    public float gravity = 9.81f;
    public float jumpAngle = 0.0f;
    public float factor = 1.0f; 
    public bool canMove = false;   

    private GameState gameState;
    private Rigidbody rb;

    private Dictionary<string, bool> CollidablePlaces = new Dictionary<string, bool>();

    public List<Checkpoint> checkpoints = new List<Checkpoint>();

    public bool didJumpMid = false;
    public bool didJumpFinal = false;

    public bool jumping = false;

    public bool moved = false;

    public Vector3 currentPosition;
    public Vector3 previousPosition;

    public bool ObjIsMoving = true;
    public bool ischecking = false;
    public bool dontAccelerate = false;

    [SerializeField]
    private bool debugWin = false;
    
    public GameObject midCollider;
    public GameObject finalCollider;

    [SerializeField]
    public BoxCollider initialPosition;

    [SerializeField]
    private float timeTaken = 0.0f;

    private void Awake() 
    {
        Vector3 boxPos = new Vector3(initialPosition.transform.position.x, transform.position.y, initialPosition.transform.position.z);
        InitializeCollidablePlaces();
        checkpoints.Add(new Checkpoint(
            boxPos,
            transform.rotation,
            factor)
        );
        transform.position = boxPos;
    }

    private void Start() 
    {
        gameState = transform.parent.GetComponent<GameState>();
        rb = GetComponent<Rigidbody>();
        rb.mass = Mass;
        canMove = gameState.States[gameState.getSimulationName()];
        currentPosition = transform.position;
        previousPosition = transform.position;
        Application.targetFrameRate = 300;
    }

    private void Update()
    {
        canMove = gameState.States[gameState.getSimulationName()];
        previousPosition = currentPosition;
        currentPosition = transform.position;

        if(gameState.States[gameState.getSimulationName()]) timeTaken += Time.deltaTime;
        
        if(debugWin){
            windWhenSelected();
            debugWin = false;
        }
    }
    
    private void FixedUpdate() 
    {
        if(canMove)
        {
            if(!dontAccelerate)
                MoveCharacter();
        }

        //if (gameState.States[gameState.getSimulationName()]) checkIfObjectIsMoving();
        //ObjIsMoving = (!rb.IsSleeping() && (rb.velocity.magnitude > 0.05f) == true);
    }

    public void MoveCharacter()
    {
        rb.AddForce(-Vector3.forward * acceleration * 30, ForceMode.Force);
        moved = true;  
    }

    private void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;

        if (!CollidablePlaces[tag])
        {
            //Collision with forbidden object
            gameState.SwitchState(gameState.getLostName());

        }
    }

    private void OnCollisionStay(Collision other)
    {
        string tag = other.gameObject.tag;

        //Debug.Log("Colidiu com " + tag);

        if(gameState.States[gameState.getExplorationName()] && checkpoints.Count == 0){
            checkpoints.Add(new Checkpoint(transform.position, transform.rotation, factor, false, false));
        }

        if(gameState.States[gameState.getSimulationName()])
        {
            if (!CollidablePlaces[tag])
            {
                //Collision with forbidden object
                Debug.Log("Colidiu com " + tag);
                gameState.SwitchState(gameState.getLostName());
            }

            else
            {
                //Collission with allowed object
                if (tag == "StartPlatform")
                {
                    if(checkpoints.Count == 0)
                    {
                        checkpoints.Add(new Checkpoint(transform.position, transform.rotation, factor, false, false));
                    }
                    else if (!ObjIsMoving)
                    {
                        gameState.SwitchState(gameState.getLostName());
                    }
                }

                if (tag == "MidPlatform")
                {
                    if(!ObjIsMoving && checkpoints.Count == 1)
                    {
                        Vector3 pos = new Vector3(midCollider.transform.position.x, midCollider.transform.position.y + 5, midCollider.transform.position.z);
                        checkpoints.Add(new Checkpoint(pos, checkpoints[0].getRotation(), 0.0f, true, false));
                        //gameState.playMessage(1);
                        Reset(checkpoints[checkpoints.Count - 1]);
                        gameState.SwitchState(gameState.getExplorationName());
                    }

                    if(checkpoints.Count == 2)
                    {
                        if (!didJumpFinal) //Se ele ainda não tiver pulado
                        { 
                            if(gameState.States[gameState.getSimulationName()])
                            {
                                Jump(false);
                                didJumpFinal = true;
                            }
                        }
                        else if(!ObjIsMoving && didJumpFinal) //Se ele já tiver pulado
                        { 
                            gameState.SwitchState(gameState.getExplorationName());
                            
                        }
                    }
                }

                if (tag == "FinalPlatform")
                {
                    if (!ObjIsMoving && checkpoints.Count == 2)
                    {    
                        checkpoints.Add(new Checkpoint(finalCollider.transform.position, checkpoints[0].getRotation(), 0.0f, true, true));
                        //gameState.playMessage(1);
                        gameState.SwitchState(gameState.getWinName());
                    }
                }
            }
        }
    }

    public void Reset()
    {
        Checkpoint lastCkp = checkpoints[checkpoints.Count - 1];
        ResetValues(lastCkp);
        ResetPosition(lastCkp);
    }

    public void Reset(Checkpoint ckp)
    {
        ResetValues(ckp);
        ResetPosition(ckp);
    }

    public void ResetValues(Checkpoint ckp)
    {
        factor = ckp.getFactor();
        didJumpMid = ckp.getJumpMid();
        didJumpFinal = ckp.getJumpFinal();
        moved = false;
        timeTaken = 0.0f;

        if(checkpoints.Count == 1){
            dontAccelerate = false;
        }
    }

    public void ResetPosition(Checkpoint checkpoint)
    {
        Debug.Log("Reset Position");

        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = checkpoint.getPosition();
        transform.rotation = checkpoint.getRotation();
        GetComponent<Rigidbody>().isKinematic = false;

        if (checkpoints.Count == 2)
        {
            didJumpFinal = false;
        }

        jumping = false;
    }

    public void stopAllMovement(){
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ResetEverythingFromScratch()
    {
        Reset(checkpoints[0]);
        checkpoints.Clear();
    }

    public void setAngle(string value)
    {
        jumpAngle = ParseValue(value);
    }

    public void setAcceleration(string value)
    {
        acceleration = ParseValue(value);
    }

    public void checkIfObjectIsMoving()
    {
        if(!ischecking)
        {
            //Debug.Log("checking obj...");
            StartCoroutine(
                CheckMoving(
                    this.gameObject, //gameobject
                    (i)=> { ObjIsMoving = i; }, //callback function that sets 'ObjIsStill'
                    (i)=> { ischecking = i; } //callback function that sets 'ischecking'
                )
            );
        }
    }

    private IEnumerator CheckMoving(GameObject gmobj, System.Action<bool> setMoveBool, System.Action<bool> setIsChecking)
    {
        setIsChecking(true);
        yield return new WaitForFixedUpdate();

        string msg;
        if(!rb.IsSleeping() && (rb.velocity.magnitude > 0.05f) == true)
        {
            msg = "(" + rb.velocity.magnitude.ToString() + ")IT IS FUCKING MOVING, RUNNNN!";
            setMoveBool(true);
        }

        else
        {
            msg = "(" + rb.velocity.magnitude.ToString() + ") WAIT! I THINK IT'S DEAD...";
            setMoveBool(false);
        }

        //Debug.Log(msg);
        
        setIsChecking(false);
        yield return 0;
    }

    private float ParseValue(string text)
    {
        float value = 0.0f;

        try
        {
            value = float.Parse(text);
        }
        catch(Exception e)
        {
            Debug.Log("Could not parse " + text);
        }

        return value;
    }

    public void Jump(bool useVz)
    {
        if(!jumping){
            float Vz = acceleration;
            if (useVz)
            {
                Vz = rb.velocity.z;
            }
            float V0 = findV0UsingVx(Vz, jumpAngle);
            float Vy = findVyUsingV0(V0, jumpAngle);

            Debug.Log("Speed: "+ rb.velocity.magnitude + "\nV0: " + V0 + "\nVz: " + Vz + "\nVy: " + Vy);
            Debug.Log("Tempo decorrido: " + timeTaken);

            Vector3 force = new Vector3(0, -Vy, 0);
            Debug.Log("force = ("+ force.x + ", "+ force.y + ", " + force.z + ")");
            Debug.Log("gravidade: " + gravity);
            float reach = (2 * Mathf.Pow(V0, 2) * Mathf.Cos(Mathf.Deg2Rad * jumpAngle) * Mathf.Sin(Mathf.Deg2Rad * jumpAngle)) / -gravity;
            Debug.Log("Alcance: " + reach);

            rb.AddForce(force,  ForceMode.VelocityChange);
            jumping = true;
        }
    }

    public float findVyUsingV0(float V0, float angle)
    {
        float angleinRad = Mathf.Deg2Rad * angle;
        Debug.Log("angleInRad[" + angle + "]: " + angleinRad);

        float angleSin = Mathf.Sin(angleinRad); 
        Debug.Log("angleSin[" + angle + "]: " + angleSin);
        float Vy = V0 * angleSin;

        return Vy;
    }

    public float findV0UsingVx(float Vx, float angle)
    {
        float angleinRad = Mathf.Deg2Rad * angle;
        float angleCos = Mathf.Cos(angleinRad); 
        Debug.Log("angleCos[" + angle + "]: " + angleCos);
        
        if (angleCos == 0.0f)
        {
            return 0.0f;
        }

        float V0 = Vx / angleCos;

        return V0;
    }

    public void zeroFactor()
    {
        StartCoroutine(
            CoroutineFactor(
                (i)=>{ factor = i; },  //callback
                0.02f  //waitTime
            )
        );
    }

    public void windWhenSelected(){
        checkpoints.Add(new Checkpoint());
        checkpoints.Add(new Checkpoint());
        gameState.SwitchState(gameState.getWinName());
    }

    public IEnumerator CoroutineFactor(System.Action<float> callback, float waitTime)
    {
        yield return new WaitForSeconds(1 * waitTime);
        callback(0.0f);
        //print("ZERO FACTOR!");
    }

    private void InitializeCollidablePlaces()
    {
        CollidablePlaces.Add("Untagged", false);
        CollidablePlaces.Add("Floor", false);
        CollidablePlaces.Add("Ceiling", true);
        CollidablePlaces.Add("Wall", true);
        CollidablePlaces.Add("StartPlatform", true);
        CollidablePlaces.Add("MidPlatform", true);
        CollidablePlaces.Add("FinalPlatform", true);
        CollidablePlaces.Add("MidPlatform-Collider", true);
        CollidablePlaces.Add("FinalPlatform-Collider", true);
        CollidablePlaces.Add("Boundaries", true);
    }

}
