using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _chaseInput;
    float _attackRange;

    public EnemyStateAttack(Animator animator, LineOfSight lOS, T chaseInput, float attackRange)
    {
        _animator = animator;
        _LOS = lOS;
        _chaseInput = chaseInput;
        this._attackRange = attackRange;
    }
    public override void Enter()
    {
        _animator.SetBool("Attacking", true);
    }
    public override void Execute()
    {
        base.Execute();
        
        Debug.Log("Enemy attack");

        if (!_LOS.HasLOS(_attackRange))
            _fsm.Transition(_chaseInput);

    }
    public override void Sleep()
    {
        _animator.SetBool("Attacking", false);
    }
}
