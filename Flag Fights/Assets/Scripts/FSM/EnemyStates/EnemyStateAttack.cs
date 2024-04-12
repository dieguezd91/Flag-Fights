using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : State<T>
{
    Animator _animator;
    LineOfSight _LOS;

    public EnemyStateAttack(Animator animator, LineOfSight lOS)
    {
        _animator = animator;
        _LOS = lOS; 
    }
    public override void Enter()
    {
        _animator.SetBool("Attacking", true);
    }
    public override void Execute()
    {
        base.Execute();
        
        Debug.Log("Enemy attack");
    }
    public override void Sleep()
    {
        _animator.SetBool("Attacking", false);
    }
}
