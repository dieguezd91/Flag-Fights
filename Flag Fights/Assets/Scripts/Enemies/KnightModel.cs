using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightModel : EnemyModel
{
    public float patrollingSpeed;
    public float timeToFind;
    public float checkCooldown;
    public float restingTime;
    public bool isIdle = true;
    public bool isFinishPath = true;
    public float lastCheck;
    public Vector3 lastTargetPosKnown;
    public AudioClip attackSFX;
    public AudioClip swingSFX;
}
