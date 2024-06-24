using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStatePatrol<T> : State<T>, IPoints
{
    Enemy _enemy;
    float changeCD = 7.5f;
    float lastChange;
    Vector3 dir;

    List<Vector3> _waypoints;
    int _nextPoint = 0;



    public EnemyStatePatrol(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.Animator.SetBool("Patrolling", true);
        _enemy.AgentController.target = GetNewTarget();
        _enemy.AgentController.RunThetaStar();
    }

    public override void Execute()
    {
        base.Execute();
        Run();


        //if (Physics.Raycast(_enemy.transform.position + Vector3.up * 1.25f, dir, 2f, _enemy.obsMask) || Time.time >= lastChange + changeCD) dir = SetNewDir();
        //Move(dir);
        //LookDir(dir);

        //Debug.DrawRay(_enemy.transform.position + Vector3.up * 1.25f, dir * 2f);
    }

    public override void Sleep()
    {
        _enemy.Animator.SetBool("Patrolling", false);
    }

    void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= _enemy.patrollingSpeed;
        dirToMove.y = _enemy.RB.velocity.y;
        _enemy.RB.velocity = dirToMove;
        
    }

    void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        _enemy.transform.forward = dirToLook;
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
        //_nextPoint = 0;
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        var pos = _waypoints[_nextPoint];
        pos.y = _enemy.transform.position.y;
        //_enemy.SetPosition(pos);
        _enemy._isFinishPath = false;
    }

    void Run()
    {
        if (_enemy._isFinishPath) return;
        var point = _waypoints[_nextPoint];
        var posPoint = point;
        posPoint.y = _enemy.transform.position.y;
        Vector3 dir = posPoint - _enemy.transform.position;
        if (dir.magnitude < 0.2f)
        {
            if (_nextPoint + 1 < _waypoints.Count)
            {
                _nextPoint++;

            }
            else
            {
                _enemy._isFinishPath = true;
                _enemy.AgentController.target = GetNewTarget();
                return;
            }
        }
        Move(_enemy.OBS.GetNewDir(dir.normalized));
        LookDir(_enemy.OBS.GetNewDir(dir));
    }

}
