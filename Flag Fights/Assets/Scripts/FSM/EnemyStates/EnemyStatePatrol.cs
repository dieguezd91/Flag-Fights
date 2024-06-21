using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>
{
    Enemy _enemy;
    float changeCD = 7.5f;
    float lastChange;
    Vector3 dir;

    public EnemyStatePatrol(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Patrolling", true);
        dir = SetNewDir();
    }

    public override void Execute()
    {
        base.Execute();
        if (Physics.Raycast(_enemy.transform.position + Vector3.up * 1.25f, dir, 2f, _enemy.obsMask) || Time.time >= lastChange + changeCD) dir = SetNewDir();
        Move(dir);
        LookDir(dir);

#if UNITY_EDITOR
        Debug.DrawRay(_enemy.transform.position + Vector3.up * 1.25f, dir * 2f);
#endif
    }

    public override void Sleep()
    {
        _enemy.Animator.SetBool("Patrolling", false);
    }
    void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= _enemy.patrollingSpeed;
        dirToMove.y = _enemy.RB.velocity.y;
        _enemy.RB.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _enemy.transform.forward = dirToLook;
    }

    Vector3 SetNewDir()
    {
        lastChange = Time.time;
        Vector3 newDir = new Vector3(MyRandoms.Range(-1, 1), 0, MyRandoms.Range(-1, 1)).normalized;
        return newDir;
    }
}
