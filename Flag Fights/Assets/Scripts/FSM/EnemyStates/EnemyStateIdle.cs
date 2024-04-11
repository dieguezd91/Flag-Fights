using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _attackInput;
    public EnemyStateIdle(Animator animator, LineOfSight LOS, T attackInput)
    {
        _LOS = LOS;
        _attackInput = attackInput;
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

        if (_LOS.HasLOS()) _fsm.Transition(_attackInput);
    }
    public override void Sleep()
    {
        _animator.SetBool("Idle", false);
    }
}
