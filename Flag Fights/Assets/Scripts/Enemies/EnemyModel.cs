using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public float ChasingSpeed = 5f;
    public float PatrollingSpeed = 3f;
    public float TimeToFind = 2f;
    public float CheckCooldown = 1f;
    public float Angle = 90f;
    public float Radius = 10f;
    public float PersonalArea = 2f;
    public float AttackRange = 1.5f;
    public float AttackCD = 1f;
    public float RestingTime = 5f;
    public bool IsIdle = true;
    public bool IsFinishPath = true;

    public AudioClip AttackSFX { get; set; }
    public AudioClip SwingSFX { get; set; }
}
