using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    T _input;

    public PlayerStateIdle(T input)
    {
        _input = input;
    }
    public override void Execute()
    {
        base.Execute();

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        if (hor != 0 || ver != 0)
        {
            //Transition
            _fsm.Transition(_input);
        }
    }
}
