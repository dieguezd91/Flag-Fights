using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateChase<T> : State<T>
{
    Enemy _enemy;
    T _patrolInput;
    T _attackInput;
    float _lastCheck;
    float _timePrediction;
    Vector3 lastPosKnown;

    public EnemyStateChase(Enemy enemy, T patrolInput, T attackInput, float timePrediction = 1)
    {
        _enemy = enemy;
        _patrolInput = patrolInput;
        _attackInput = attackInput;
        _timePrediction = timePrediction;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Chasing", true);
    }

    public override void Execute()
    {
        if (_enemy.LOS.HasLOS() && Time.time >= _lastCheck + _enemy.checkCooldown)
        {
            lastPosKnown = _enemy.LOS.TargetLOS.position;
            _lastCheck = Time.time;                                                                         //Save last time it has seen the enemy
        }
        if (_enemy.LOS.HasLOS(_enemy.attackRange)) _fsm.Transition(_attackInput);                                       //If is close enought to the player, entry Attack State
        else if (!_enemy.LOS.HasLOS() && Time.time >= _lastCheck + _enemy.timeToFind) _fsm.Transition(_patrolInput);    //If it has not LOS to the player and the last time it had LOS to them,
        else                                                                                                //entry Patrol State
        {
            Vector3 dir = _enemy.OBS.GetNewDir(GetDir());
            Move(dir);
            LookDir(new Vector3(dir.x, 0, dir.z));
        }
    }

    public override void Sleep()
    {
        _enemy.Animator.SetBool("Chasing", false);
    }

    private Vector3 GetDir()
    {
        Rigidbody target = _enemy.LOS.TargetLOS.GetComponent<Rigidbody>();
        Vector3 point = lastPosKnown + target.transform.forward * 2 * _timePrediction;
        Vector3 dirToPoint = (point - _enemy.transform.position).normalized;
        Vector3 dirToTarget = (lastPosKnown - _enemy.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return dirToPoint.normalized;
    }

    void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= _enemy.chasingSpeed;
        dirToMove.y = _enemy.RB.velocity.y;
        _enemy.RB.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _enemy.transform.forward = dirToLook;
    }
}
