using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody _rb;
    public bool hasLineOfSight = false;
    public Vector2 targetDirection;
    public Vector2 directionToPlayer;
    protected Transform target;
    public LineOfSight _lineOfSight;
    public float speed;
    public int life;
    public bool isIdle;
    public float attackRange;
    ITreeNode _root;

    FSM<EnemyStatesEnum> _fsm;

    private void Awake()
    {
        InitializeFSM(); 
        InitializeTree(); 
    }

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _lineOfSight = GetComponent<LineOfSight>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    void InitializeFSM()
    {
        var idle = new EnemyStateIdle<EnemyStatesEnum>(_lineOfSight, EnemyStatesEnum.Attack);
        var patrol = new EnemyStatePatrol<EnemyStatesEnum>();
        var chase = new EnemyStateChase<EnemyStatesEnum>();
        var attack = new EnemyStateAttack<EnemyStatesEnum>();

        _fsm = new FSM<EnemyStatesEnum>(idle);

        idle.AddTransition(EnemyStatesEnum.Patrol, patrol);
        idle.AddTransition(EnemyStatesEnum.Chase, chase);
        idle.AddTransition(EnemyStatesEnum.Attack, attack);
        patrol.AddTransition(EnemyStatesEnum.Chase, chase);
        patrol.AddTransition(EnemyStatesEnum.Attack, attack);
        chase.AddTransition(EnemyStatesEnum.Patrol, patrol);
        chase.AddTransition(EnemyStatesEnum.Attack, attack);
        attack.AddTransition(EnemyStatesEnum.Chase, chase);
        attack.AddTransition(EnemyStatesEnum.Patrol, patrol);
    }

    void InitializeTree()
    {
        //Actions
        ITreeNode dead = new ActionNode(() => print("Tree: Dead"));
        ITreeNode idle = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Idle));
        ITreeNode patrol = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Patrol));
        ITreeNode chase = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Chase));
        ITreeNode attack = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Attack));

        //Questions
        ITreeNode qChase = new QuestionNode(QuestionIsOnRange, attack, chase);
        ITreeNode qPatrol = new QuestionNode(QuestionLoS, qChase, patrol);
        ITreeNode qIdle = new QuestionNode(QuestionIdle, idle, qPatrol);
        ITreeNode qHasLife = new QuestionNode(QuestionHasLife, qIdle, dead);

        _root = qHasLife;
    }

    public void ChangeTree(ITreeNode newTree) => _root = newTree;
    public bool QuestionLoS() => _lineOfSight.HasLineOfSight();
    public bool QuestionHasLife() => life > 0;
    public bool QuestionIsOnRange() => _lineOfSight.HasLineOfSight(attackRange);
    public bool QuestionIdle() => isIdle;
}
