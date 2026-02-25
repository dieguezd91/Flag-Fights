using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    KnightController _enemyController;
    KnightView _view;
    KnightModel _model;
    float restStartTime;

    public EnemyStateIdle(KnightController enemyController, KnightModel model, KnightView view)
    {
        _enemyController = enemyController;
        _model = model; 
        _view = view;
    }

    public override void Enter()
    {
        if (_enemyController == null)
        {
            return;
        }
        if (_view._animator == null)
        {
            return;
        }

        _view._animator.SetBool("Idle", true);
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
        restStartTime = Time.time;
    }

    public override void Execute()
    {
        base.Execute();
        if (Time.time >= _model.restingTime + restStartTime)
            _model.isFinishPath = false;
    }

    public override void Sleep()
    {
        _view._animator.SetBool("Idle", false);
    }
}

