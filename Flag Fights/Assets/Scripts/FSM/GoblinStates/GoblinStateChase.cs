using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class GoblinStateChase<T> : State<T>
{
    Goblin _enemy;

    public GoblinStateChase(Goblin enemy)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = _enemy.OBS.GetNewDir(_enemy.Steering.GetDir());
        _enemy.Move(dir);
        _enemy.LookDir(new Vector3(dir.x, 0, dir.z));
    }

    public override void Sleep()
    {
        _enemy.Animator.SetBool("Running", false);
    }
}
