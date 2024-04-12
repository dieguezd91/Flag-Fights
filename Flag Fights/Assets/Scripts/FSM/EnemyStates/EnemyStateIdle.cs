using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _patrolInput;

    public EnemyStateIdle(Animator animator, LineOfSight LOS, T patrolInput)
    {
        _LOS = LOS;
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

        if (_LOS.HasLOS()) _fsm.Transition(_patrolInput);
    }
    
    public override void Sleep()
    {
        _animator.SetBool("Idle", false);
    }
}
