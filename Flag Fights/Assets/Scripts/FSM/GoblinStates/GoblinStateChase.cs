using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinStateChase<T> : State<T>
{
    GoblinController _controller;
    GoblinModel _model;
    GoblinView _view;

    public GoblinStateChase(GoblinController controller, GoblinView view, GoblinModel model)
    {
        _controller = controller;
        _view = view;
        _model = model;
    }

    public override void Enter()
    {
        _view._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = _view.OBS.GetNewDir(_controller.Steering.GetDir());
        _view.Move(dir, _model.Speed);
        _view.LookDir(new Vector3(dir.x, 0, dir.z));
    }

    public override void Sleep()
    {
        _view._animator.SetBool("Running", false);
    }
}
