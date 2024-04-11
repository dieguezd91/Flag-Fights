using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack<T> : StateMono<T>
{
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy attack");
    }
}
