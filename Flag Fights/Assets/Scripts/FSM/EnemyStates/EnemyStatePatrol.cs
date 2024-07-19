using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>, IPoints
{
    KnightController _enemyController;
    KnightView _view;
    KnightModel _model;
    float lastChange;
    Vector3 dir;

    List<Vector3> _waypoints;
    int _nextPoint = 0;

    public EnemyStatePatrol(KnightController enemyController, KnightModel model, KnightView view)
    {
        _enemyController = enemyController;
        _view = view;
        _model = model;
    }

    public override void Enter()
    {
        if (_view == null || _view._animator == null || _view.AgentController == null)
        {
            return;
        }

        _view._animator.SetBool("Patrolling", true);

        var newTarget = GetNewTarget();
        if (newTarget == null)
        {
            Debug.Log("No se encontro un nuevo objetivo");
            return;
        }

        _view.AgentController.target = newTarget;

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
        if (_view == null || _view._animator == null)
        {
            return;
        }

        _view._animator.SetBool("Patrolling", false);
    }

    void Move(Vector3 dirToMove)
    {
        if (_view == null || _view.RB == null)
        {
            return;
        }

        dirToMove *= _model.patrollingSpeed;
        dirToMove.y = _view.RB.velocity.y;
        _view.RB.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)
    {
        if (_view == null)
        {
            return;
        }

        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _enemyController.transform.forward = dirToLook;
    }

    Node GetNewTarget()
    {
        var r = MyRandoms.Range(0, GameManager.instance.Nodes.Count);
        return GameManager.instance.Nodes[(int)r];
    }

    public void SetWayPoints(List<Node> newPoints)
    {
        var list = new List<Vector3>();

        for (int i = 0; i < newPoints.Count; i++)
        {
            list.Add(newPoints[i].transform.position);
        }
        SetWayPoints(list);
    }

    public void SetWayPoints(List<Vector3> newPoints)
    {
        if (newPoints == null || newPoints.Count == 0)
        {
            return;
        }

        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _enemyController.transform.position.y;
        _enemyController.Model.isFinishPath = false;
    }

    void Run()
    {
        if (_model.isFinishPath || _waypoints == null || _waypoints.Count == 0) return;

        var point = _waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = _enemyController.transform.position.y;
        Vector3 dir = posPoint - _enemyController.transform.position;
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

        Move(_view.ObstacleAvoidance.GetNewDir(dir.normalized));
        LookDir(_view.ObstacleAvoidance.GetNewDir(dir));
    }
}




