using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateChase<T> : State<T>
{
    private EnemyController controller;
    private float lastCheck;
    private float timePrediction;
    private Vector3 lastPosKnown;

    public EnemyStateChase(EnemyController controller, float timePrediction = 1)
    {
        this.controller = controller;
        this.timePrediction = timePrediction;
    }

    public override void Enter()
    {
        controller.View._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        if (controller.View.LineOfSight.HasLOS() && Time.time >= lastCheck + controller.Model.CheckCooldown)
        {
            lastPosKnown = controller.View.LineOfSight.TargetLOS.position;
            lastCheck = Time.time;
        }
        else
        {
            Vector3 dir = controller.View.ObstacleAvoidance.GetNewDir(GetDir());
            controller.View.Move(dir, controller.Model.ChasingSpeed);
            controller.View.LookDir(new Vector3(dir.x, 0, dir.z));
        }
    }

    public override void Sleep()
    {
        controller.View._animator.SetBool("Running", false);
    }

    private Vector3 GetDir()
    {
        Rigidbody target = controller.View.LineOfSight.TargetLOS.GetComponent<Rigidbody>();
        Vector3 point = lastPosKnown + target.transform.forward * 2 * timePrediction;
        Vector3 dirToPoint = (point - controller.View.transform.position).normalized;
        Vector3 dirToTarget = (lastPosKnown - controller.View.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return dirToPoint.normalized;
    }
}
