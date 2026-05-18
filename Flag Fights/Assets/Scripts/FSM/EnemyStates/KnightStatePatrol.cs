using System.Collections.Generic;
using UnityEngine;

public class KnightStatePatrol<T> : State<T>, IPoints
{
    KnightController _controller;
    KnightView _view;
    KnightModel _model;

    List<Vector3> _waypoints;
    int _nextPoint = 0;

    public KnightStatePatrol(KnightController controller, KnightModel model, KnightView view)
    {
        _controller = controller;
        _view = view;
        _model = model;
    }

    public override void Enter()
    {
        if (_view._animator != null)
            _view._animator.SetBool("Patrolling", true);
        _view.AgentController.target = GetNewTarget();
        _nextPoint = 0;
        _view.AgentController.RunThetaStar();
    }

    public override void Execute()
    {
        base.Execute();
        Run();
    }

    public override void Sleep()
    {
        if (_view._animator != null)
            _view._animator.SetBool("Patrolling", false);
    }

    void Move(Vector3 dirToMove)
    {
        dirToMove *= _model.patrollingSpeed;
        dirToMove.y = _view.RB.velocity.y;
        _view.RB.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _controller.transform.forward = dirToLook;
    }

    Node GetNewTarget()
    {
        return _controller.NodeRegistry?.GetRandomNode();
    }

    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < newPoints.Count; i++)
            list.Add(newPoints[i].transform.position);
        SetWayPoints(list);
    }

    public void SetWayPoints(List<Vector3> newPoints)
    {
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _controller.transform.position.y;
        _controller.Model.isFinishPath = false;
    }

    void Run()
    {
        if (_model.isFinishPath) return;
        if (_waypoints == null || _waypoints.Count == 0)
        {
            _view.AgentController.RunThetaStar();
            return;
        }
        var posPoint = _waypoints[_nextPoint];
        posPoint.y = _controller.transform.position.y;
        Vector3 dir = posPoint - _controller.transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < _waypoints.Count)
            {
                _nextPoint++;
            }
            else
            {
                _model.isFinishPath = true;
                _view.AgentController.target = GetNewTarget();
                return;
            }
        }
        Vector3 avoidDir = _view.ObstacleAvoidance.GetNewDir(dir.normalized);
        Move(avoidDir);
        LookDir(avoidDir);
    }
}
