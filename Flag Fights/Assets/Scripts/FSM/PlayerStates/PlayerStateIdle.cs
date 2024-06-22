using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateIdle<T> : State<T>
{
    private PlayerView _view;
    private PlayerModel _model;
    private T _runInput;

    public PlayerStateIdle(PlayerView view, PlayerModel model, T runInput)
    {
        _view = view;
        _model = model;
        _runInput = runInput;
    }

    public override void Enter()
    {
        _view.Animator.SetBool("Idle", true);
    }

    public override void Execute()
    {
        base.Execute();
        if (_model.MovementInput != Vector2.zero)
        {
            _fsm.Transition(_runInput);
        }
    }

    public override void Sleep()
    {
        _view.Animator.SetBool("Idle", false);
    }
}
