using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    Enemy _enemy;

    public float restStartTime;

    public EnemyStateIdle(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Idle", true);
        restStartTime = Time.time;
       
    }

    public override void Execute()
    {
        base.Execute();
        if (Time.time >= _enemy.restingTime + restStartTime)
            _enemy._isFinishPath = false;
    }
    
    public override void Sleep()
    {
        _enemy.Animator.SetBool("Idle", false);

    }
}
