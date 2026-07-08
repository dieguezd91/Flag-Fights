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

    [Header("Attack Settings")]
    public float attackImpactDelay = 0.35f;
    public float attackRecoveryTime = 0.8f;
    public float hitForwardOffset = 0.35f;
    public float hitVerticalOffset = 0.5f;
    public float hitRadius = 0.4f;
    public LayerMask hitLayerMask;

    [Header("Attack Debug")]
    public bool debugAttack = false;
}
