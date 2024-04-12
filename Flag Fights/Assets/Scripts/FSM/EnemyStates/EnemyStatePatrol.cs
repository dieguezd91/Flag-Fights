using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _chaseInput;

    public EnemyStatePatrol(Animator animator, LineOfSight LOS, T chaseInput)
    {
        _animator = animator;
        _LOS = LOS;
        _chaseInput = chaseInput;
    }

    public override void Enter()
    {
        _animator.SetBool("Patrolling", true);
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy patrol");
    }

    public override void Sleep()
    {
        _animator.SetBool("Patrolling", false);
    }
}
