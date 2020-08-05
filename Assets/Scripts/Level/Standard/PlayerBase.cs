using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    protected float _mass = 50.0f;
    protected float _acceleration = 5f;
    protected float _jumpAngle = 45.0f;
    protected float _gravity = 9.81f;

    public abstract void Run();
    public abstract void Jump();
    
    public abstract float Mass { get; set; }
    public abstract float Acceleration { get;set; }
    public abstract float JumpAngle { get;set; }
    public abstract float Gravity { get;set; }
}
