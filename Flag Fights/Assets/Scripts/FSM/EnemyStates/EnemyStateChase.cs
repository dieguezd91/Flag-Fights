using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : State<T>
{
    Animator _animator;
    public EnemyStateChase(Animator animator)
    {
        _animator = animator;
    }
    public override void Enter()
    {
        _animator.SetBool("Chase", true);
    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy chase");
    }
    public override void Sleep()
    {
        _animator.SetBool("Chase", false);
    }
}
