using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateChase<T> : State<T>
{
    Animator _animator;
    Transform _transform;
    Rigidbody _rb;
    LineOfSight _LOS;
    T _patrolInput;
    T _attackInput;
    float _attackRange;
    float _speed;
    float _timeToFind;
    float _checkCooldown;
    float _lastCheck;
    float _timePrediction;
    Vector3 lastPosKnown;
    ObstacleAvoidance _obs;

    public EnemyStateChase(Animator animator, Transform transform, Rigidbody rb, LineOfSight LOS, ObstacleAvoidance obs, T patrolInput, T attackInput, float attackRange, float speed, float timeToFind, float checkCooldown, float timePrediction = 1)
    {
        _animator = animator;
        _transform = transform;
        _rb = rb;
        _LOS = LOS;
        _obs = obs;
        _patrolInput = patrolInput;
        _attackInput = attackInput;
        _attackRange = attackRange;
        _speed = speed;
        _timeToFind = timeToFind;
        _checkCooldown = checkCooldown;
        _timePrediction = timePrediction;
    }

    public override void Enter()
    {
        _animator.SetBool("Chasing", true);
    }

    public override void Execute()
    {
        if (_LOS.HasLOS() && Time.time >= _lastCheck + _checkCooldown)
        {
            lastPosKnown = _LOS.TargetLOS.position;
            _lastCheck = Time.time;                                                                         //Save last time it has seen the enemy
        }
        if (_LOS.HasLOS(_attackRange)) _fsm.Transition(_attackInput);                                       //If is close enought to the player, entry Attack State
        else if (!_LOS.HasLOS() && Time.time >= _lastCheck + _timeToFind) _fsm.Transition(_patrolInput);    //If it has not LOS to the player and the last time it had LOS to them,
        else                                                                                                //entry Patrol State
        {;
            Vector3 dirToPoint = _obs.GetNewDir(GetDir());
            Move(dirToPoint);
            LookDir(new Vector3(dirToPoint.x, 0, dirToPoint.z));
        }
    }

    public override void Sleep()
    {
        _animator.SetBool("Chasing", false);
    }

    private Vector3 GetDir()
    {
        Rigidbody target = _LOS.TargetLOS.GetComponent<Rigidbody>();
        Vector3 point = target.position + target.transform.forward * 2 * _timePrediction;
        Vector3 dirToPoint = (point - _transform.position).normalized;
        Vector3 dirToTarget = (target.position - _transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return dirToPoint;
    }

    void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= _speed;
        dirToMove.y = _rb.velocity.y;
        _rb.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _transform.forward = dirToLook;
    }
}
