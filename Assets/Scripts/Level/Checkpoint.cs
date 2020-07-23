using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint
{
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Quaternion rotation;
    private float factor = 0.0f;
    private bool didJumpMid = false;
    private bool didJumpFinal = false;

    public Checkpoint(){
        this.position = new Vector3();
        this.rotation = new Quaternion();
        this.factor = 0.0f;
    }

    public Checkpoint(Vector3 position){
        this.position = position;
        this.rotation = new Quaternion();
        this.factor = 0.0f;
    }

    public Checkpoint(Vector3 position, Quaternion rotation){
        this.position = position;
        this.rotation = rotation;
        this.factor = 0.0f;
    }

    public Checkpoint(Vector3 position, Quaternion rotation, float factor){
        this.position = position;
        this.rotation = rotation;
        this.factor = factor;
    }

    public Checkpoint(Vector3 position, Quaternion rotation, float factor, bool didJumpMid, bool didJumpFinal){
        this.position = position;
        this.rotation = rotation;
        this.factor = factor;
        this.didJumpMid = didJumpMid;
        this.didJumpFinal = didJumpFinal;
    }


    public Vector3 getPosition(){
        return position;
    }

    public Quaternion getRotation(){
        return rotation;
    }

    public float getFactor(){
        return factor;
    }

    public bool getJumpMid(){
        return didJumpMid;
    }

    public bool getJumpFinal(){
        return didJumpFinal;
    }

    public void setPosition(Vector3 position){
        this.position = position;
    }

    public void setRotation(Quaternion rotation){
        this.rotation = rotation;
    }

    public void setFactor(float factor){
        this.factor = factor;
    }

    public void setJumpMid(bool jump){
        didJumpMid = jump;
    }

    public void setJumpFinal(bool jump){
        didJumpFinal = jump;
    }
}
