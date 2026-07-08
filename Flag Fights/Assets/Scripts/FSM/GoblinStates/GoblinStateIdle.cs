using UnityEngine;

public class GoblinStateIdle<T> : State<T>
{
    GoblinController _controller;
    GoblinView _view;
    GoblinModel _model;

    public GoblinStateIdle(GoblinController controller, GoblinModel model, GoblinView view)
    {
        _controller = controller;
        _model = model;
        _view = view;
    }

    public override void Enter()
    {
        if (_controller == null || _view._animator == null) return;

        _view._animator.SetBool("Idle", true);
        _view._animator.SetBool("Running", false);
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
    }

    public override void Execute()
    {
        base.Execute();
        if (_view._animator != null)
        {
            _view._animator.SetBool("Idle", true);
            _view._animator.SetBool("Running", false);
        }
    }
}
