using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _patrolInput;
    T _attackInput;
    float _attackRange;

    public EnemyStateChase(Animator animator, LineOfSight LOS, T patrolInput, T attackInput, float attackRange)
    {
        _animator = animator;
        _LOS = LOS;
        _patrolInput = patrolInput;
        _attackInput = attackInput;
        _attackRange = attackRange;
    }
    public override void Enter()
    {
        _animator.SetBool("Chasing", true);
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy chase");

        if (_LOS.HasLOS(_attackRange))
            _fsm.Transition(_attackInput);
        else if (!_LOS.HasLOS())
            _fsm.Transition(_patrolInput);
        
    }
    public override void Sleep()
    {
        _animator.SetBool("Chasing", false);
    }
}
