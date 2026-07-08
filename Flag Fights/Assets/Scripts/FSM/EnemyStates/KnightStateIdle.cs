using UnityEngine;

public class KnightStateIdle<T> : State<T>
{
    KnightController _controller;
    KnightView _view;
    KnightModel _model;
    float restStartTime;

    public KnightStateIdle(KnightController controller, KnightModel model, KnightView view)
    {
        _controller = controller;
        _model = model;
        _view = view;
    }

    public override void Enter()
    {
        if (_controller == null) return;
        if (_view._animator == null) return;

        _view._animator.SetBool("Idle", true);
        _view._animator.SetBool("Running", false);
        _view._animator.SetBool("Patrolling", false);
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
        restStartTime = Time.time;
    }

    public override void Execute()
    {
        base.Execute();
        if (_view._animator != null)
        {
            _view._animator.SetBool("Idle", true);
            _view._animator.SetBool("Running", false);
            _view._animator.SetBool("Patrolling", false);
        }
        if (Time.time >= _model.restingTime + restStartTime)
            _model.isFinishPath = false;
    }
}
