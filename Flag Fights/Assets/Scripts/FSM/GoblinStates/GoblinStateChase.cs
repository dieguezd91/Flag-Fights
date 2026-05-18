using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinStateChase<T> : State<T>
{
    GoblinController _controller;
    GoblinModel _model;
    GoblinView _view;

    public GoblinStateChase(GoblinController controller, GoblinModel model, GoblinView view)
    {
        _controller = controller;
        _model = model;
        _view = view;
    }

    public override void Enter()
    {
        if (_view._animator != null)
            _view._animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        Vector3 dir = _view.OBS.GetNewDir(_controller.Steering.GetDir(), false);
        _view.Move(dir, _model.chasingSpeed);
        _view.LookDir(new Vector3(dir.x, 0, dir.z));
    }

    public override void Sleep()
    {
        if (_view._animator != null)
            _view._animator.SetBool("Running", false);
    }
}
