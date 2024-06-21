using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRun<T> : State<T>
{
    PlayerController _player;

    public PlayerStateRun(PlayerController player)
    {
        _player = player;
    }

    public override void Enter()
    {
        _player.Animator.SetBool("Running", true);
    }
    public override void Execute()
    {
        _player.transform.Translate(Vector3.forward * Time.deltaTime * _player.Speed * _player.MovementInput.y);
        _player.transform.Rotate(Vector3.up, _player.TurnSpeed * _player.MovementInput.x * Time.deltaTime);
    }
    public override void Sleep()
    {
        _player.Animator.SetBool("Running", false);
    }
}
