using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    Animator _animator;
    float _speed;
    T _runInput;

    public PlayerStateIdle(Animator animator, float speed, T input)
    {
        _runInput = input;
        _animator = animator;
    }

    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        base.Execute();
        _animator.SetFloat("Speed", _speed);
        float hor = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");

        if (hor != 0 || fwd != 0) _fsm.Transition(_runInput);
    }

    public override void Sleep()
    {

    }
}
