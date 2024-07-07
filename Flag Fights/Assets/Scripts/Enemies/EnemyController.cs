using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyModel Model { get; private set; }
    public EnemyView View { get; private set; }

    private FSM<EnemyStatesEnum> fsm;
    private ITreeNode root;

    private EnemyStatePatrol<EnemyStatesEnum> _enemyPatrol;

    private void Awake()
    {
        Model = GetComponent<EnemyModel>();
        View = GetComponent<EnemyView>();

        InitializeFSM();
        InitializeTree();
    }

    private void Update()
    {
        fsm.OnUpdate();
        root.Execute();
    }

    public void InitializeFSM()
    {
        _enemyPatrol = new EnemyStatePatrol<EnemyStatesEnum>(this);
        var chase = new EnemyStateChase<EnemyStatesEnum>(this);
        var attack = new EnemyStateAttack<EnemyStatesEnum>(this);
        var idle = new EnemyStateIdle<EnemyStatesEnum>(this);

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

    private void InitializeTree()
    {
        ITreeNode patrol = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Patrol));
        ITreeNode chase = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Chase));
        ITreeNode attack = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Attack));
        ITreeNode idle = new ActionNode(() => fsm.Transition(EnemyStatesEnum.Idle));

        ITreeNode qPatrol = new QuestionNode(QPatrol, patrol, idle);
        ITreeNode qChase = new QuestionNode(QChase, chase, qPatrol);
        ITreeNode qAttack = new QuestionNode(QAttack, attack, qChase);

        root = qAttack;
    }

    public bool QAttack() => View.LineOfSight.HasLOS(Model.AttackRange);
    public bool QChase() => View.LineOfSight.HasLOS(View.LineOfSight.Vision);
    public bool QPatrol() => !Model.IsFinishPath;

    public IPoints GetStateWaypoints => _enemyPatrol;
}
