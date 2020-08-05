using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : PlayerBase
{
    public References references;
    private Dictionary<string, bool> CollidablePlaces = new Dictionary<string, bool>();

    [SerializeField]
    public override float Mass 
    { 
        get => _mass;
        set => _mass = value; 
    }
    [SerializeField]
    public override float Acceleration 
    {
        get => _acceleration;
        set => _acceleration = value;
    }
    [SerializeField]
    public override float JumpAngle
    {
        get => _jumpAngle;
        set => _jumpAngle = value;
    }
    [SerializeField]
    public override float Gravity
    {
        get => _gravity;
        set => _gravity = value;
    }

    [HideInInspector]
    public List<Checkpoint> Checkpoints = new List<Checkpoint>();
    public int StartPlatformPosition = 1;
    private Animator PlayerAnimator;
    private Rigidbody PlayerRigidbody;
    private GameState GSReference;
    [SerializeField]
    private float UnitScale = 0.5492f; //the amount of units to relative to a meter.
    private int MovementDirection = -1;
    [SerializeField]
    private float TimeSpanned = 0.0f;
    [SerializeField]
    private bool IsMoving = false;
    [SerializeField]
    private bool IsJumping = false;
    [SerializeField]
    private bool JumpedMid = false;
    [SerializeField]
    private int jumpstates = 0;
    [HideInInspector]
    public string RunAnimationName = "is_running";
    [HideInInspector]
    public string JumpAnimationName = "is_going_upwards";
    [HideInInspector]
    public string WinAnimationName = "did_win";
    private Vector3 CurrentPosition;
    private Vector3 PreviousPosition;
    private float MovementRange = 0.0000005f; // gap in which movement is not considered. 
    public bool IsPlayerOnInitialPlatform = true;



    public override void Run()
    {
        PlayerAnimator.SetBool(RunAnimationName, true);
        if(!PlayerAnimator.GetBool(RunAnimationName))
        {
            PlayerAnimator.Play("Base Layer.Running",  0, 0.0f);   
        }
        PlayerRigidbody.AddForce(MovementDirection * Vector3.forward * _acceleration * UnitScale, ForceMode.Force);
    }

    public override void Jump()
    {
        if(!IsJumping){
            Debug.Log("Jump!");
            Debug.LogFormat("TimeSpanned = {0} s", TimeSpanned);
            IsJumping = true;
            StopAnimation(RunAnimationName);
            PlayerAnimator.SetBool(JumpAnimationName, true);
            PlayerRigidbody.AddForce(GetJumpVector(), ForceMode.VelocityChange);
        }
    }

    private Vector3 GetJumpVector()
    {
        float Vx;
        float Vy;
        float Vz;
        float V;

        float angleInRad = _jumpAngle * Mathf.Deg2Rad;
        float angleCos = Mathf.Cos(angleInRad);
        float angleSin = Mathf.Sin(angleInRad);

        if (IsMoving)
        {
            Vz = PlayerRigidbody.velocity.z;
        }
        else
        {
            V = _acceleration;
            Vz = V * angleCos;
        }

        if(angleCos == 0.0f)
        {
            V = 0;
        }
        else
        {
            V = Vz / angleCos;
        }

        Vy = V * angleSin;
        Vx = 0.0f;

        float reach = (2 * Mathf.Pow(V, 2) * angleCos * angleSin) / _gravity;

        Vx = Mathf.Abs(Vx);
        Vy = Mathf.Abs(Vy);
        Vz = Mathf.Abs(Vz);
        V = Mathf.Abs(V);

        Debug.LogFormat("Vx = {0} m/s,  Vy = {1} m/s,  Vz = {2} m/s,  V = {3} m/s", Vx, Vy, Vz, V);
        Debug.LogFormat("Alcance = {0} m", reach);

        Vector3 JumpVector = new Vector3(Vx, Vy, -Vz);

        if (IsMoving)
        {
            JumpVector.z = 0.0f;
        }

        //Debug.LogFormat("JumpVector = {0}", JumpVector.ToString());

        return JumpVector;
    }

    public void StopAnimation(string animationName)
    {
        PlayerAnimator.SetBool(animationName, false);
    }

    public void StopMovement()
    {
        PlayerRigidbody.velocity = Vector3.zero;
        PlayerRigidbody.angularVelocity = Vector3.zero;
    }

    private void CheckIfObjectIsMoving()
    {
        // If the object did not move
        if( // Lower limit
            (CurrentPosition.x >= PreviousPosition.x - MovementRange) &&
            (CurrentPosition.y >= PreviousPosition.y - MovementRange) &&
            (CurrentPosition.z >= PreviousPosition.z - MovementRange) &&
            // Upper limit
            (CurrentPosition.x <= PreviousPosition.x + MovementRange) &&
            (CurrentPosition.y <= PreviousPosition.y + MovementRange) &&
            (CurrentPosition.z <= PreviousPosition.z + MovementRange))
        {
            IsMoving = false;
        }
        else
        {
            IsMoving = true;    
        }
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

    public void ResetEverythingFromScratch()
    {
        Debug.Log("Resetting from scratch");
        Reset(Checkpoints[0]);
        Checkpoints.Clear();
    }

    public void Reset(Checkpoint ckp)
    {
        Debug.Log("Reset() called.");
        StopMovement();
        ResetAnimation();
        ResetValues(ckp);
        ResetPosition(ckp);
    }

    public void ResetPosition(Checkpoint ckp)
    {
        //Debug.Log("ResetPosition() called.");

        StopMovement();
        PlayerRigidbody.isKinematic = true;
        transform.position = ckp.getPosition();
        transform.rotation = ckp.getRotation();
        PlayerRigidbody.isKinematic = false;
    }

    public void ResetValues(Checkpoint ckp)
    {
        //Debug.Log("ResetValues() called.");
        IsPlayerOnInitialPlatform = ckp.GetIsPlayerOnInitialPlatform();
        JumpedMid = ckp.getJumpedMid();
        TimeSpanned = 0.0f;
        IsJumping = false;
        jumpstates = ckp.jumpstates;
    }

    public void ResetAnimation()
    {
        //Debug.Log("ResetAnimation() called.");

        StopAnimation(RunAnimationName);
        StopAnimation(JumpAnimationName);
        PlayerAnimator.Play("Base Layer.Idle", 0, 0.0f);
    }

    public void setAngle(string value)
    {
        _jumpAngle = ParseValue(value);
    }

    public void setAcceleration(string value)
    {
        _acceleration = ParseValue(value);
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
            Debug.Log(e.ToString() + ": Could not parse " + text);
        }

        return value;
    }




    private void Awake()
    {
        InitializeCollidablePlaces();
    }

    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        GSReference = references.GameState.GetComponent<GameState>();

        Physics.gravity = new Vector3(0.0f, -_gravity, 0.0f);
    }

    private void Update()
    {
        if(GSReference.States[GSReference.getSimulationName()])
        {
            TimeSpanned += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        CheckIfObjectIsMoving();
        if (Checkpoints.Count > 1) 
        {
            ChangePlayerPosition.canChangePosition = false;
        }

        PreviousPosition = CurrentPosition;
        CurrentPosition = transform.position;

        if ( GSReference.States[GSReference.getSimulationName()] )
        {
            if (IsPlayerOnInitialPlatform)
            {
                Run();
            }

            if (Checkpoints.Count == 2 && !JumpedMid && jumpstates == 1)
            {
                Jump();
                JumpedMid = true;
                jumpstates = 2;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        string tag = other.transform.tag;
        string name = other.transform.name;

        //Debug.LogFormat("tag = {0}  name = {1}", tag, name);

        if ((name == "PM - Collider" && Checkpoints.Count == 1) || (name == "PF - Collider" && Checkpoints.Count == 2))
        {
            //Debug.Log("Colidiu com um dos colliders das outras plataformas.");
            StopMovement();
            IsJumping = false;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        string tag = other.transform.tag;
        string name = other.transform.name;

        //Debug.LogFormat("tag = {0}  name = {1}", tag, name);

        if(CollidablePlaces[tag])
        {
            // If collided with authorized object
            if (tag == "StartPlatform")
            {
                if (Checkpoints.Count == 0)
                {
                    Checkpoint ckp = new Checkpoint(transform.position, transform.rotation, IsPlayerOnInitialPlatform, JumpedMid);
                    ckp.jumpstates = jumpstates;
                    Checkpoints.Add(ckp);
                }
            }
        }
        else{
            // If collided with forbidden object
            GSReference.SwitchState(GSReference.getLostName());
        }
    }

    private void OnCollisionStay(Collision other)
    {
        string tag = other.transform.tag;
        bool onSimulation = GSReference.States[GSReference.getSimulationName()];

        if(CollidablePlaces[tag]) // If is colliding with authorized object
        {
            if (tag == "StartPlatform") // If is colliding with the start platform                
            {
                
            }
            else if (tag == "MidPlatform") // If is colliding with the middle platform
            {
                if (!IsMoving) // If object is not moving
                {
                    if (Checkpoints.Count == 1) // if there's only one checkpoint, add another
                    {
                        IsPlayerOnInitialPlatform = false;
                        jumpstates = 1;
                        Checkpoint ckp = new Checkpoint(transform.position, transform.rotation, IsPlayerOnInitialPlatform, JumpedMid);
                        ckp.jumpstates = jumpstates;
                        Checkpoints.Add(ckp);
                        //GSReference.playMessage(1);

                        references.ExplorerCamera.transform.position = new Vector3(
                            references.PlayerBackCamera.transform.position.x,
                            references.PlayerBackCamera.transform.position.y + 5.0f,
                            references.PlayerBackCamera.transform.position.z + 15.0f 
                        );

                        Reset(Checkpoints[Checkpoints.Count - 1]);
                        GSReference.SwitchState(GSReference.getExplorationName());
                        ChangePlayerPosition.canChangePosition = false;
                    }
                }
            }
            else if (tag == "FinalPlatform") // If is colliding with the final platform
            {
                if (!IsMoving)
                {
                    if (Checkpoints.Count == 2)
                    {
                        Checkpoint ckp = new Checkpoint(transform.position, transform.rotation, IsPlayerOnInitialPlatform, JumpedMid);
                        Checkpoints.Add(ckp);
                        //GSReference.playMessage(1);
                        GSReference.SwitchState(GSReference.getWinName());   
                    }
                }
            }

        }
        else // If is colliding with forbidden object
        {
            GSReference.SwitchState(GSReference.getLostName());
        }
    }

}
