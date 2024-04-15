using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _chaseInput;
    T _patrolInput;

    public EnemyStateIdle(Animator animator, LineOfSight LOS, T chaseInput, T patrolInput)
    {
        _LOS = LOS;
        _chaseInput = chaseInput;
        _patrolInput = patrolInput;
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.SetBool("Idle", true);
    }

    public override void Execute()
    {
        base.Execute();

        Debug.Log("Enemy idle");

        if (_LOS.HasLOS()) _fsm.Transition(_chaseInput);
        else _fsm.Transition(_patrolInput);
    }
    
    public override void Sleep()
    {
        _animator.SetBool("Idle", false);
    }
}
