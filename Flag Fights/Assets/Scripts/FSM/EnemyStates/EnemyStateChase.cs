using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Apple;

public class EnemyStateChase<T> : State<T>
{
    private KnightController controller;
    private KnightModel model;
    private KnightView view;
    private float timePrediction;

    public EnemyStateChase(KnightController controller, KnightModel model, KnightView view, float timePrediction = 1)
    {
        this.controller = controller;
        this.timePrediction = timePrediction;
        this.model = model;
        this.view = view;
    }

    public override void Enter()
    {
        controller.View._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = controller.View.ObstacleAvoidance.GetNewDir(GetDir());
        view.Move(dir, controller.Model.chasingSpeed);
        view.LookDir(new Vector3(dir.x, 0, dir.z));
    }

    public override void Sleep()
    {
        controller.View._animator.SetBool("Running", false);
    }

    private Vector3 GetDir()
    {
        Vector3 knownPos = controller.Model.lastTargetPosKnown;
        Transform targetTransform = controller.View.LineOfSight.TargetLOS;
        Vector3 point = knownPos + targetTransform.forward * 2 * timePrediction;
        Vector3 dirToPoint = (point - controller.View.transform.position).normalized;
        Vector3 dirToTarget = (knownPos - controller.View.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return dirToPoint.normalized;
    }
}
