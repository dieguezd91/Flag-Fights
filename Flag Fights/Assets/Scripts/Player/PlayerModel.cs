using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour 
{
    public bool HasFlag { get; set; }
    public float Speed { get; set; }
    public float TurnSpeed { get; set; }
    public Vector2 MovementInput { get; set; }

    public PlayerModel(float speed, float turnSpeed)
    {
        HasFlag = false;
        Speed = speed;
        TurnSpeed = turnSpeed;
        MovementInput = Vector2.zero;
    }
}
