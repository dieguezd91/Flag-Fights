using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateChase<T> : State<T>
{
    Enemy _enemy;
    float _lastCheck;
    float _timePrediction;
    Vector3 lastPosKnown;

    public EnemyStateChase(Enemy enemy, float timePrediction = 1)
    {
        _enemy = enemy;
        _timePrediction = timePrediction;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        if (_enemy.LOS.HasLOS() && Time.time >= _lastCheck + _enemy.checkCooldown)
        {
            lastPosKnown = _enemy.LOS.TargetLOS.position;
            _lastCheck = Time.time;                                                                         //Save last time it has seen the enemy
        }
        else                                                                                //entry Patrol State
        {
            Vector3 dir = _enemy.OBS.GetNewDir(GetDir());
            _enemy.Move(dir);
            _enemy.LookDir(new Vector3(dir.x, 0, dir.z));
        }
    }

    public override void Sleep()
    {
        _enemy.Animator.SetBool("Running", false);
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
}
