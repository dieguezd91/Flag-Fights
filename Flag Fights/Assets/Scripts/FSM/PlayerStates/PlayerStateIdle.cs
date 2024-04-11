using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    T _walkInput;

    public PlayerStateIdle(T input)
    {
        _walkInput = input;
    }
    public override void Execute()
    {
        base.Execute();

        float hor = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");

        if (hor != 0 || fwd != 0) _fsm.Transition(_walkInput);
    }
}
