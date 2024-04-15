using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStateChase<T> : State<T>
{
    Animator _animator;
    Transform _transform;
    Rigidbody _rb;
    LineOfSight _LOS;
    T _patrolInput;
    T _attackInput;
    float _attackRange;
    float _speed;
    float _timeToFind;
    float _lastCheck;

    public EnemyStateChase(Animator animator, Transform transform, Rigidbody rb, LineOfSight LOS, T patrolInput, T attackInput, float attackRange, float speed, float timeToFind)
    {
        _animator = animator;
        _transform = transform;
        _rb = rb;
        _LOS = LOS;
        _patrolInput = patrolInput;
        _attackInput = attackInput;
        _attackRange = attackRange;
        _speed = speed;
        _timeToFind = timeToFind;
    }
    public override void Enter()
    {
        _animator.SetBool("Chasing", true);
    }
    public override void Execute()
    {
        if (_LOS.HasLOS()) _lastCheck = Time.time;
        if (_LOS.HasLOS(_attackRange)) _fsm.Transition(_attackInput);
        else if (!_LOS.HasLOS() && Time.time >= _lastCheck + _timeToFind) _fsm.Transition(_patrolInput);
        else
        {
            Vector3 dir = _LOS.TargetLOS.position - _transform.position;

            Move(dir.normalized);
            LookDir(dir.normalized);
        }
    }
    public override void Sleep()
    {
        _animator.SetBool("Chasing", false);
    }

    void Move(Vector3 dirToMove)
    {
        dirToMove *= _speed;
        dirToMove.y = _rb.velocity.y;
        _rb.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _transform.forward = dirToLook;
    }
}
