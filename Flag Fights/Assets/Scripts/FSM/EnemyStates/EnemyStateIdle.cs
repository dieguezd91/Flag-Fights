using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyStateIdle<T> : State<T>
{
    LineOfSight _LOS;
    T _attackInput;
    public EnemyStateIdle(LineOfSight LOS, T attackInput)
    {
        _LOS = LOS;
        _attackInput = attackInput;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy idle");

        if (_LOS.HasLineOfSight()) _fsm.Transition(_attackInput);
    }
}
