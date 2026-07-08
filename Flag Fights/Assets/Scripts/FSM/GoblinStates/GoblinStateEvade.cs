using UnityEngine;

public class GoblinStateEvade<T> : State<T>
{
    GoblinController _controller;
    GoblinView _view;
    GoblinModel _model;
    float _timePrediction;

    public GoblinStateEvade(GoblinController enemy, GoblinModel model, GoblinView view, float timePrediction = 2)
    {
        _controller = enemy;
        _model = model;
        _view = view;
        _timePrediction = timePrediction;
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
        Vector3 dir = _view.OBS.GetNewDir(GetDir(), false);
        _view.Move(dir, _model.chasingSpeed);
        _view.LookDir(new Vector3(dir.x, 0, dir.z));
    }


    private Vector3 GetDir()
    {
        Transform target = _view.LOS.TargetLOS;
        Vector3 point = target.position + target.forward * 2 * _timePrediction;
        Vector3 dirToPoint = (point - _controller.transform.position).normalized;
        Vector3 dirToTarget = (target.position - _controller.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return -dirToPoint;
    }
}
