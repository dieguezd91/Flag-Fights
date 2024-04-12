using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;
    T _attackInput;

    public EnemyStateChase(Animator animator, LineOfSight LOS, T attackInput)
    {
        _animator = animator;
        _LOS = LOS;
        _attackInput = attackInput;

    }
    public override void Enter()
    {
        _animator.SetBool("Chasing", true);
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy chase");
    }
    public override void Sleep()
    {
        _animator.SetBool("Chasing", false);
    }
}
