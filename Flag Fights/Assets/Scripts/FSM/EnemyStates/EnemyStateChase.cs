using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase<T> : State<T>
{
    public override void Execute()
    {
        base.Execute();
        Debug.Log("Enemy chase");
    }
}
