using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinStateEvade<T> : State<T>
{
    GoblinController _controller;
    GoblinView _view;
    GoblinModel _model;
    float _timePrediction;

    public GoblinStateEvade(GoblinController enemy, GoblinModel model, GoblinView view, float timePrediction = 2)
    {
        _controller = enemy;
        _model = model;
        _view = view;
        _timePrediction = timePrediction;
    }

    public override void Enter()
    {
        _view._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = _view.OBS.GetNewDir(GetDir(), false);
        _view.Move(dir, _model.chasingSpeed);
        _view.LookDir(new Vector3(dir.x, 0, dir.z));
    }

    public override void Sleep()
    {
        _view._animator.SetBool("Running", false);
    }


    private Vector3 GetDir()
    {
        Transform target = _view.LOS.TargetLOS;
        Vector3 point = target.position + target.forward * 2 * _timePrediction;
        Vector3 dirToPoint = (point - _controller.transform.position).normalized;
        Vector3 dirToTarget = (target.position - _controller.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return -dirToPoint;
    }
}
