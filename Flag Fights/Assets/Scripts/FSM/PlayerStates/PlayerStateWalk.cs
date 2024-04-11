using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWalk<T> : State<T>
{
    float _speed;
    float _turnSpeed;
    Transform _transform;
    T _idleInput;

    public PlayerStateWalk(float speed, float turnSpeed, Transform transform, T input)
    {
        _transform = transform;
        _idleInput = input;
        _speed = speed;
        _turnSpeed= turnSpeed;
    }

    public override void Execute()
    {
        float hor = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");

        _transform.Translate(Vector3.forward * Time.deltaTime * _speed * fwd);
        _transform.Rotate(Vector3.up, _turnSpeed * hor * Time.deltaTime);

        if (hor == 0 && fwd == 0) _fsm.Transition(_idleInput);
    }
}
