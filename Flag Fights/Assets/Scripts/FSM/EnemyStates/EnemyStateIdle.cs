using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    Enemy _enemy;
    T _chaseInput;
    T _patrolInput;

    public EnemyStateIdle(Enemy enemy, T chaseInput, T patrolInput)
    {
        _chaseInput = chaseInput;
        _patrolInput = patrolInput;
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Idle", true);
    }

    public override void Execute()
    {
        base.Execute();

        if (_enemy.LOS.HasLOS()) _fsm.Transition(_chaseInput);            //If it has LOS to the player, entry Chase State
        else _fsm.Transition(_patrolInput);                         // If it has not LOS to the player, entry Patrol State
    }
    
    public override void Sleep()
    {
        _enemy.Animator.SetBool("Idle", false);
    }
}
