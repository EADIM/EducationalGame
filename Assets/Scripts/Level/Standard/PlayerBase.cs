using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    protected float _mass = 50.0f;
    protected float _acceleration = 0.0f;
    protected float _jumpAngle = 0.0f;
    protected float _gravity = 9.81f;
    protected float _jumpForce = 0f;

    public abstract void Run();
    public abstract void Jump(Vector3 vector, ForceMode mode);
    
    public abstract float Mass { get; set; }
    public abstract float Acceleration { get;set; }
    public abstract float JumpAngle { get;set; }
    public abstract float Gravity { get;set; }
    public abstract float JumpForce { get;set; }
}
