using System.Collections.Generic;
using UnityEngine;

public class EnemyStatePatrol<T> : State<T>, IPoints
{
    EnemyController _enemyController;
    float changeCD = 7.5f;
    float lastChange;
    Vector3 dir;

    List<Vector3> _waypoints;
    int _nextPoint = 0;

    public EnemyStatePatrol(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }

    public override void Enter()
    {
        _enemyController.View._animator.SetBool("Patrolling", true);
        _enemyController.View.AgentController.target = GetNewTarget();
        _nextPoint = 0;
        _enemyController.View.AgentController.RunThetaStar();
    }

    public override void Execute()
    {
        base.Execute();
        Run();
    }

    public override void Sleep()
    {
        _enemyController.View._animator.SetBool("Patrolling", false);
    }

    void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= _enemyController.Model.PatrollingSpeed;
        dirToMove.y = _enemyController.View.RB.velocity.y;
        _enemyController.View.RB.velocity = dirToMove;
    }

    void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
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
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _enemyController.transform.position.y;
        _enemyController.Model.IsFinishPath = false;
    }

    void Run()
    {
        if (_enemyController.Model.IsFinishPath) return;
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
                _enemyController.Model.IsFinishPath = true;
                _enemyController.View.AgentController.target = GetNewTarget();
                return;
            }
        }
        Move(_enemyController.View.ObstacleAvoidance.GetNewDir(dir.normalized));
        LookDir(_enemyController.View.ObstacleAvoidance.GetNewDir(dir));
    }
}

