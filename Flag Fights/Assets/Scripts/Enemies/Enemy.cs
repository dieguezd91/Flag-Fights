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

    public float restingTime;

    //AI
    FSM<EnemyStatesEnum> _fsm;
    ITreeNode _root;

    AgentController agentController;
    public AgentController AgentController => agentController;

    public bool _isFinishPath = true;

    EnemyStatePatrol<EnemyStatesEnum> _enemyPatrol;

    private void Awake()
    {
        _los = GetComponent<LineOfSight>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _obs = new ObstacleAvoidance(transform, angle, radius, obsMask, personalArea);
        _audioSource = GetComponent<AudioSource>();
        agentController = GetComponent<AgentController>();
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
        _enemyPatrol = new EnemyStatePatrol<EnemyStatesEnum>(this);
        var chase = new EnemyStateChase<EnemyStatesEnum>(this);
        var attack = new EnemyStateAttack<EnemyStatesEnum>(this);
        var idle = new EnemyStateIdle<EnemyStatesEnum>(this);


            //Create Finite State Machine
        _fsm = new FSM<EnemyStatesEnum>(idle);

        //Creating transition between states
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

    void InitializeTree()
    {
        //Actions
        ITreeNode patrol = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Patrol));
        ITreeNode chase = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Chase));
        ITreeNode attack = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Attack));
        ITreeNode idle = new ActionNode(() => _fsm.Transition(EnemyStatesEnum.Idle));

        //Questions
        ITreeNode qPatrol = new QuestionNode(QPatrol, patrol, idle);
        ITreeNode qChase = new QuestionNode(QChase, chase, qPatrol);
        ITreeNode qAttack = new QuestionNode(QAttack, attack, qChase);

        //First node to execute
        _root = qAttack;
    }

    public bool QAttack() => _los.HasLOS(attackRange);
    public bool QChase() => _los.HasLOS(_los.Vision);
    public bool QPatrol() => !_isFinishPath;

    public void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= chasingSpeed;
        dirToMove.y = RB.velocity.y;
        RB.velocity = dirToMove;
    }

    public void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        transform.forward = dirToLook;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public IPoints GetStateWaypoints => _enemyPatrol;
}
