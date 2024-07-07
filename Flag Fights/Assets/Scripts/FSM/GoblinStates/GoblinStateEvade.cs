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

    public GoblinStateEvade(GoblinController enemy, GoblinView view, GoblinModel model, float timePrediction = 2)
    {
        _controller = enemy;
        _view = view;
        _model = model;
        _timePrediction = timePrediction;
    }

    public override void Enter()
    {
        _view._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = _view.OBS.GetNewDir(GetDir());
        _view.Move(dir, _model.Speed);
        _view.LookDir(dir);
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
