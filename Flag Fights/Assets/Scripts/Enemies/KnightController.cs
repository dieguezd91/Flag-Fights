using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class KnightController : EnemyController
{
    [HideInInspector] public KnightModel Model;
    [HideInInspector] public KnightView View;

    private FSM<EnemyStatesEnum> fsm;

    private EnemyStatePatrol<EnemyStatesEnum> _enemyPatrol;

    public override void Awake()
    {
        Model = GetComponent<KnightModel>();
        View = GetComponent<KnightView>();
        Model.lastTargetPosKnown = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        InitializeFSM();
        InitializeTree();
    }

    public override void Update()
    {
        fsm.OnUpdate();
        _root.Execute();
    }

    public override void InitializeFSM()
    {
        _enemyPatrol = new EnemyStatePatrol<EnemyStatesEnum>(this, Model, View);
        var chase = new EnemyStateChase<EnemyStatesEnum>(this, Model, View);
        var attack = new EnemyStateAttack<EnemyStatesEnum>(this, Model, View);
        var idle = new EnemyStateIdle<EnemyStatesEnum>(this, Model, View);

        fsm = new FSM<EnemyStatesEnum>(idle);

        _enemyPatrol.AddTransition(EnemyStatesEnum.Chase, chase);
        _enemyPatrol.AddTransition(EnemyStatesEnum.Attack, attack);
        _enemyPatrol.AddTransition(EnemyStatesEnum.Idle, idle);
        chase.AddTransition(EnemyStatesEnum.Patrol, _enemyPatrol);
        chase.AddTransition(EnemyStatesEnum.Idle, idle);
        chase.AddTransition(EnemyStatesEnum.Attack, attack);
        attack.AddTransition(EnemyStatesEnum.Chase, chase);
        attack.AddTransition(EnemyStatesEnum.Patrol, _enemyPatrol);
        attack.AddTransition(EnemyStatesEnum.Idle, idle);
        idle.AddTransition(EnemyStatesEnum.Chase, chase);
        idle.AddTransition(EnemyStatesEnum.Patrol, _enemyPatrol);
        idle.AddTransition(EnemyStatesEnum.Attack, attack);
    }

    public override void InitializeTree()
    {
        ITreeNode patrol = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Patrol));
        ITreeNode chase = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Chase));
        ITreeNode attack = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Attack));
        ITreeNode idle = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Idle));

        ITreeNode qPatrol = new QuestionNode(QPatrol, patrol, idle);
        ITreeNode qChase = new QuestionNode(QChase, chase, qPatrol);
        ITreeNode qAttack = new QuestionNode(QAttack, attack, qChase);

        _root = qAttack;
    }

    public bool QAttack() => View.LineOfSight.HasLOS(Model.attackRange);

    public bool QChase()
    {
        if (View.LineOfSight.HasLOS(View.LineOfSight.Vision) || View.LineOfSight.HasLOS(View.LineOfSight.Vision, Model.lastTargetPosKnown))
        {
            if (Time.time >= Model.lastCheck + Model.checkCooldown)
            {
                Model.lastTargetPosKnown = View.LineOfSight.TargetLOS.position;
                Model.lastCheck = Time.time;
            }
            return true;
        }
        else return false;

    }
    public bool QPatrol() => !Model.isFinishPath;

    public IPoints GetStateWaypoints => _enemyPatrol;
}
