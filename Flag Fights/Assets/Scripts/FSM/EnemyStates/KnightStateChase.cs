using UnityEngine;

public class KnightStateChase<T> : State<T>
{
    KnightController _controller;
    KnightModel _model;
    KnightView _view;
    float _timePrediction;

    public KnightStateChase(KnightController controller, KnightModel model, KnightView view, float timePrediction = 1)
    {
        _controller = controller;
        _model = model;
        _view = view;
        _timePrediction = timePrediction;
    }

    private void UpdateAnimator()
    {
        if (_controller.View._animator == null) return;
        float speed = new Vector3(_controller.View.RB.velocity.x, 0, _controller.View.RB.velocity.z).magnitude;
        if (speed > 0.1f)
        {
            _controller.View._animator.SetBool("Running", true);
            _controller.View._animator.SetBool("Patrolling", false);
            _controller.View._animator.SetBool("Idle", false);
        }
        else
        {
            _controller.View._animator.SetBool("Idle", true);
            _controller.View._animator.SetBool("Running", false);
            _controller.View._animator.SetBool("Patrolling", false);
        }
    }

    public override void Enter()
    {
        UpdateAnimator();
    }

    public override void Execute()
    {
        UpdateAnimator();
        Vector3 dir = _controller.View.ObstacleAvoidance.GetNewDir(GetDir());
        _view.Move(dir, _controller.Model.chasingSpeed);
        _view.LookDir(new Vector3(dir.x, 0, dir.z));
    }

    Vector3 GetDir()
    {
        Vector3 knownPos = _controller.Model.lastTargetPosKnown;
        Transform targetTransform = _controller.View.LineOfSight.TargetLOS;
        Vector3 point = knownPos + targetTransform.forward * 2 * _timePrediction;
        Vector3 dirToPoint = (point - _controller.View.transform.position).normalized;
        Vector3 dirToTarget = (knownPos - _controller.View.transform.position).normalized;
        if (Vector3.Dot(dirToPoint, dirToTarget) < 0) dirToPoint = dirToTarget;
        return dirToPoint.normalized;
    }
}
