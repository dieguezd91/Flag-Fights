using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : StateMono<T>
{
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy patrol");
    }
}
