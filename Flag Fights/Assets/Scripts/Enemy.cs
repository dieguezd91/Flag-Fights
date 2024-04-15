using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //COMPONENTS
    Animator _animator;
    LineOfSight _lineOfSight;

    //STATS
    public float speed;
    public bool isIdle;
    public float attackRange;

    //AI
    FSM<EnemyStatesEnum> _fsm;
    ITreeNode _root;

    private void Awake()
    {
        _lineOfSight = GetComponent<LineOfSight>();
        _animator = GetComponent<Animator>();
        InitializeFSM(); 
        InitializeTree(); 
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
    }

    void InitializeFSM()
    {
            //Declarating states
        var idle = new EnemyStateIdle<EnemyStatesEnum>(_animator, _lineOfSight, EnemyStatesEnum.Chase, EnemyStatesEnum.Patrol);
        var patrol = new EnemyStatePatrol<EnemyStatesEnum>(_animator, _lineOfSight, EnemyStatesEnum.Chase, EnemyStatesEnum.Attack, attackRange);
        var chase = new EnemyStateChase<EnemyStatesEnum>(_animator, _lineOfSight, EnemyStatesEnum.Patrol, EnemyStatesEnum.Attack, attackRange);
        var attack = new EnemyStateAttack<EnemyStatesEnum>(_animator, _lineOfSight, EnemyStatesEnum.Chase, attackRange);

            //Create Finite State Machine
        _fsm = new FSM<EnemyStatesEnum>(idle);

            //Creating transition between states
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
        ITreeNode idle = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Idle));
        ITreeNode patrol = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Patrol));
        ITreeNode chase = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Chase));
        ITreeNode attack = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Attack));

        //Questions
        ITreeNode qChase = new QuestionNode(QuestionIsOnRange, attack, chase);
        ITreeNode qPatrol = new QuestionNode(QuestionLoS, qChase, patrol);
        ITreeNode qIdle = new QuestionNode(QuestionIdle, idle, qPatrol);

        //First node to execute
        _root = qIdle;
    }
    public bool QuestionIsOnRange() => _lineOfSight.HasLOS(attackRange);
    public bool QuestionLoS() => _lineOfSight.HasLOS();
    public bool QuestionIdle() => isIdle;
}
