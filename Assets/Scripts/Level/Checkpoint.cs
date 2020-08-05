using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint
{
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;
    private bool IsPlayerOnInitialPlatform = true;
    private bool JumpedMid = false;
    public int jumpstates;

    public Checkpoint(){
        this.position = new Vector3();
        this.rotation = new Quaternion();
    }

    public Checkpoint(Vector3 position){
        this.position = position;
        this.rotation = new Quaternion();
    }

    public Checkpoint(Vector3 position, Quaternion rotation){
        this.position = position;
        this.rotation = rotation;
    }

    public Checkpoint(Vector3 position, Quaternion rotation, bool IsPlayerOnInitialPlatform){
        this.position = position;
        this.rotation = rotation;
        this.IsPlayerOnInitialPlatform = IsPlayerOnInitialPlatform;
    }

    public Checkpoint(Vector3 position, Quaternion rotation, bool IsPlayerOnInitialPlatform, bool JumpedMid){
        this.position = position;
        this.rotation = rotation;
        this.IsPlayerOnInitialPlatform = IsPlayerOnInitialPlatform;
        this.JumpedMid = JumpedMid;
    }


    public Vector3 getPosition(){
        return position;
    }

    public Quaternion getRotation(){
        return rotation;
    }

    public bool getJumpedMid(){
        return JumpedMid;
    }

    public bool GetIsPlayerOnInitialPlatform(){
        return IsPlayerOnInitialPlatform;
    }

    public void setPosition(Vector3 position){
        this.position = position;
    }

    public void setRotation(Quaternion rotation){
        this.rotation = rotation;
    }

    public void setJumpedMid(bool jump){
        JumpedMid = jump;
    }

    public void setIsPlayerOnInitialPlatform(bool value)
    {
        IsPlayerOnInitialPlatform = value;
    }
}
