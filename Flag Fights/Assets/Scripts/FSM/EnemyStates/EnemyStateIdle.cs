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
        // Verificar que _enemyController no sea nulo
        if (_enemyController == null)
        {
            Debug.LogError("_enemyController no está inicializado.");
            return;
        }

        // Verificar que _enemyController.View.Animator no sea nulo
        if (_enemyController.View.Animator == null)
        {
            Debug.LogError("_enemyController.View.Animator no está inicializado.");
            return;
        }

        _enemyController.View.Animator.SetBool("Idle", true);
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
        _enemyController.View.Animator.SetBool("Idle", false);
    }
}

