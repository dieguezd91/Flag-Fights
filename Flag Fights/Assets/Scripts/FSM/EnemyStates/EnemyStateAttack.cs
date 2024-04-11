using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : State<T>
{
    Animator _animator;
    public EnemyStateAttack(Animator animator)
    {
        _animator = animator;
    }
    public override void Enter()
    {
        _animator.SetBool("Attack", true);
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy attack");
    }
    public override void Sleep()
    {
        _animator.SetBool("Attack", false);
    }
}
