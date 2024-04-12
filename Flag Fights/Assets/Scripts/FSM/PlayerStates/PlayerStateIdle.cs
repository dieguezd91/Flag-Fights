using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    T _runInput;

    public PlayerStateIdle(T input)
    {
        _runInput = input;
    }

    public override void Execute()
    {
        base.Execute();

        float hor = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");

        if (hor != 0 || fwd != 0) _fsm.Transition(_runInput);
    }

}
