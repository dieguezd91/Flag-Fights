using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinStateEvade<T> : State<T>
{
    Goblin _enemy;
    float _timePrediction;

    public GoblinStateEvade(Goblin enemy, float timePrediction = 2)
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
        Vector3 dir = _enemy.OBS.GetNewDir(GetDir());
        _enemy.Move(dir);
        _enemy.LookDir(dir);
    }

    public override void Sleep()
    {
        _enemy.Animator.SetBool("Running", false);
    }


    private Vector3 GetDir()
    {
        Transform target = _enemy.LOS.TargetLOS;
        Vector3 point = target.position + target.forward * 2 * _timePrediction;
        Vector3 dirToPoint = (point - _enemy.transform.position).normalized;
        Vector3 dirToTarget = (target.position - _enemy.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return -dirToPoint;
    }
}
