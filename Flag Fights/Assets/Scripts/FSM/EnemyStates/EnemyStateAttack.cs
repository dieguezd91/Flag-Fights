using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _chaseInput;
    float _attackRange;
    float _cooldownTime = 3.0f;
    float _lastAttackTime;

    public EnemyStateAttack(Animator animator, LineOfSight lOS, T chaseInput, float attackRange)
    {
        _animator = animator;
        _LOS = lOS;
        _chaseInput = chaseInput;
        this._attackRange = attackRange;
    }

    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        base.Execute();

        if (Time.time - _lastAttackTime >= _cooldownTime)
        {
            _animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
            DealDamage();
        }

        if (!_LOS.HasLOS(_attackRange))
            _fsm.Transition(_chaseInput);
    }

    void DealDamage()
    {
        Debug.Log("Dañado");
        GameManager.instance.LoseRound();
    }

    public override void Sleep()
    {
        
    }
}
