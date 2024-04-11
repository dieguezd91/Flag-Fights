using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>
{
    Animator _animator;
    public EnemyStatePatrol(Animator animator)
    {
        _animator = animator;
    }
    public override void Enter()
    {
        _animator.SetBool("Patrol", true);
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy patrol");
    }
    public override void Sleep()
    {
        _animator.SetBool("Patrol", false);
    }
}
