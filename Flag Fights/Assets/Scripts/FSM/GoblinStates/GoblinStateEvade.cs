using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinStateEvade<T> : State<T>
{
    GoblinController _controller;
    float _timePrediction;

    public GoblinStateEvade(GoblinController enemy, float timePrediction = 2)
    {
        _controller = enemy;
        _timePrediction = timePrediction;
    }

    public override void Enter()
    {
        _controller.View._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = _controller.View.OBS.GetNewDir(GetDir());
        _controller.View.Move(dir, _controller.Model.Speed);
        _controller.View.LookDir(dir);
    }

    public override void Sleep()
    {
        _controller.View._animator.SetBool("Running", false);
    }


    private Vector3 GetDir()
    {
        Transform target = _controller.View.LOS.TargetLOS;
        Vector3 point = target.position + target.forward * 2 * _timePrediction;
        Vector3 dirToPoint = (point - _controller.transform.position).normalized;
        Vector3 dirToTarget = (target.position - _controller.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return -dirToPoint;
    }
}
