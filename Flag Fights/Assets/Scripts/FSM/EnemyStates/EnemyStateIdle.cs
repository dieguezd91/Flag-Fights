using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    EnemyController _enemyController;
    float restStartTime;

    public EnemyStateIdle(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }

    public override void Enter()
    {
        if (_enemyController == null)
        {
            return;
        }
        if (_enemyController.View._animator == null)
        {
            return;
        }

        _enemyController.View._animator.SetBool("Idle", true);
        restStartTime = Time.time;
    }

    public override void Execute()
    {
        base.Execute();
        if (Time.time >= _enemyController.Model.RestingTime + restStartTime)
            _enemyController.Model.IsFinishPath = false;
    }

    public override void Sleep()
    {
        _enemyController.View._animator.SetBool("Idle", false);
    }
}

