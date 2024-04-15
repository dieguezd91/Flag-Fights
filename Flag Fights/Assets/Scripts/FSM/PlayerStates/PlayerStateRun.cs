using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRun<T> : State<T>
{
    Animator _animator;
    float _speed;
    float _turnSpeed;
    Transform _transform;
    T _idleInput;

    public PlayerStateRun(Animator animator, float speed, float turnSpeed, Transform transform, T input)
    {
        _animator = animator;
        _speed = speed;
        _turnSpeed= turnSpeed;
        _transform = transform;
        _idleInput = input;
    }

    public override void Execute()
    {
        float hor = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");

        _animator.SetFloat("Speed", _speed);

        _transform.Translate(Vector3.forward * Time.deltaTime * _speed * fwd);
        _transform.Rotate(Vector3.up, _turnSpeed * hor * Time.deltaTime);

        if (hor == 0 || fwd == 0) _fsm.Transition(_idleInput);
    }
}
