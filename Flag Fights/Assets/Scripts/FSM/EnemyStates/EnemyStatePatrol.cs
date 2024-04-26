using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>
{
    Animator _animator;
    Transform _transform;
    Rigidbody _rb;
    LineOfSight _LOS;
    T _chaseInput;
    T _attackInput;
    float _attackRange;
    float _speed;
    float changeCD = 7.5f;
    float lastChange;
    Vector3 dir;

    public EnemyStatePatrol(Animator animator, Transform transform, Rigidbody rb, LineOfSight LOS, T chaseInput, T attackInput, float attackRange, float speed)
    {
        _animator = animator;
        _transform = transform;
        _rb = rb;
        _LOS = LOS;
        _chaseInput = chaseInput;
        _attackInput = attackInput;
        _attackRange = attackRange;
        _speed = speed;
    }

    public override void Enter()
    {
        _animator.SetBool("Patrolling", true);
        dir = SetNewDir();
    }

    public override void Execute()
    {
        base.Execute();
        Debug.DrawRay(_transform.position + Vector3.up * 1.25f, dir * 2f);

        if (Physics.Raycast(_transform.position + Vector3.up * 1.25f, dir, 2f) || Time.time >= lastChange + changeCD) dir = SetNewDir();
        Move(dir);
        LookDir(dir);

         if (_LOS.HasLOS()) _fsm.Transition(_chaseInput);                           //If it has LOS to the player, entry Chase State
        else if (_LOS.HasLOS(_attackRange)) _fsm.Transition(_attackInput);          //If it is close enought to the player, entry Attack State
    }

    public override void Sleep()
    {
        _animator.SetBool("Patrolling", false);
    }
    void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= _speed;
        dirToMove.y = _rb.velocity.y;
        _rb.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _transform.forward = dirToLook;
    }

    Vector3 SetNewDir()
    {
        lastChange = Time.time;
        Vector3 newDir = new Vector3(MyRandoms.Range(-1, 1), 0, MyRandoms.Range(-1, 1)).normalized;
        Debug.Log($"New dir: {newDir}");
        return newDir;
    }
}
