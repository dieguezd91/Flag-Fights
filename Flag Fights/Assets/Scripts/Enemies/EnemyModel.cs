using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public float ChasingSpeed;
    public float PatrollingSpeed;
    public float TimeToFind;
    public float CheckCooldown;
    public float Angle;
    public float Radius;
    public float PersonalArea;
    public float AttackRange;
    public float AttackCD;
    public float RestingTime;
    public bool IsIdle = true;
    public bool IsFinishPath = true;
    public Vector3 LastTargetPosKnown;
    public float lastCheck;

    public AudioClip AttackSFX;
    public AudioClip SwingSFX;
}
