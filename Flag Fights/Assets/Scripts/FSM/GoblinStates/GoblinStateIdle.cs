using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinStateIdle<T> : State<T>
{
    Goblin _enemy;

    public GoblinStateIdle(Goblin enemy)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Idle", true);
    }
    
    public override void Sleep()
    {
        _enemy.Animator.SetBool("Idle", false);
    }
}
