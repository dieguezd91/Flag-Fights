using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _chaseInput;
    T _attackInput;
    float _attackRange;

    public EnemyStatePatrol(Animator animator, LineOfSight LOS, T chaseInput, T attackInput, float attackRange)
    {
        _animator = animator;
        _LOS = LOS;
        _chaseInput = chaseInput;
        _attackInput = attackInput;
        _attackRange = attackRange;
    }

    public override void Enter()
    {
        _animator.SetBool("Patrolling", true);
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy patrol");


         if (_LOS.HasLOS())
            _fsm.Transition(_chaseInput);
        else if (_LOS.HasLOS(_attackRange))
            _fsm.Transition(_attackInput);
    }

    public override void Sleep()
    {
        _animator.SetBool("Patrolling", false);
    }
}
