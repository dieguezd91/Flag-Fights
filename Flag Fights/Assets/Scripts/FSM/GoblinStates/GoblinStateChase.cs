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

    private void UpdateAnimator()
    {
        if (_view._animator == null) return;
        float speed = new Vector3(_view.RB.velocity.x, 0, _view.RB.velocity.z).magnitude;
        if (speed > 0.1f)
        {
            _view._animator.SetBool("Running", true);
            _view._animator.SetBool("Idle", false);
        }
        else
        {
            _view._animator.SetBool("Idle", true);
            _view._animator.SetBool("Running", false);
        }
    }

    public override void Enter()
    {
        UpdateAnimator();
    }

    public override void Execute()
    {
        UpdateAnimator();
        Vector3 dir = _view.OBS.GetNewDir(_controller.Steering.GetDir(), false);
        _view.Move(dir, _model.chasingSpeed);
        _view.LookDir(new Vector3(dir.x, 0, dir.z));
    }
}
