using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class GoblinStateChase<T> : State<T>
{
    GoblinController _enemy;

    public GoblinStateChase(GoblinController enemy)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.View._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = _enemy.View.OBS.GetNewDir(_enemy.Steering.GetDir());
        _enemy.View.Move(dir, _enemy.Model.Speed);
        _enemy.View.LookDir(new Vector3(dir.x, 0, dir.z));
    }

    public override void Sleep()
    {
        _enemy.View._animator.SetBool("Running", false);
    }
}
