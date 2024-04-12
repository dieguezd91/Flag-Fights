using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    Animator _animator;
    T _runInput;

    public PlayerStateIdle(Animator animator, T input)
    {
        _runInput = input;
        _animator = animator;
    }

    public override void Enter()
    {
        _animator.SetBool("Idle", true);
    }

    public override void Execute()
    {
        base.Execute();

        float hor = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");

        if (hor != 0 || fwd != 0) _fsm.Transition(_runInput);
    }

    public override void Sleep()
    {
        _animator.SetBool("Idle", false);
    }
}
