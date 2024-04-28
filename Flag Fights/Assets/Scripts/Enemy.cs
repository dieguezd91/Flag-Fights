using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //COMPONENTS
    Animator _animator;
    public Animator Animator => _animator;
    Rigidbody _rb;
    public Rigidbody RB=> _rb;
    LineOfSight _los;
    public LineOfSight LOS=> _los;
    ObstacleAvoidance _obs;
    public ObstacleAvoidance OBS => _obs;
    AudioSource _audioSource;
    public AudioSource AudioSource=> _audioSource;

    //STATS
    public float chasingSpeed;
    public float patrollingSpeed;
    public float timeToFind;
    public float checkCooldown;
    public float angle;
    public float radius;
    public float personalArea;
    public LayerMask obsMask;
    public bool isIdle;
    public float attackRange;
    public float attackCD;
    public AudioClip attackSFX;
    public AudioClip swingSFX;

    //AI
    FSM<EnemyStatesEnum> _fsm;
    ITreeNode _root;

    private void Awake()
    {
        _los = GetComponent<LineOfSight>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _obs = new ObstacleAvoidance(transform, angle, radius, obsMask, personalArea);
        _audioSource = GetComponent<AudioSource>();
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
        var idle = new EnemyStateIdle<EnemyStatesEnum>(this, EnemyStatesEnum.Chase, EnemyStatesEnum.Patrol);
        var patrol = new EnemyStatePatrol<EnemyStatesEnum>(this, EnemyStatesEnum.Chase, EnemyStatesEnum.Attack);
        var chase = new EnemyStateChase<EnemyStatesEnum>(this, EnemyStatesEnum.Patrol, EnemyStatesEnum.Attack);
        var attack = new EnemyStateAttack<EnemyStatesEnum>(this, EnemyStatesEnum.Chase);

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
    public bool QuestionIsOnRange() => _los.HasLOS(attackRange);
    public bool QuestionLoS() => _los.HasLOS();
    public bool QuestionIdle() => isIdle;
}
