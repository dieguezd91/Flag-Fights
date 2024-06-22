using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

public class Goblin : MonoBehaviour, IBoid
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
    [SerializeField] float _attackRange;
    public float Speed;
    public float angle;
    public float radius;
    public float personalArea;
    public LayerMask obsMask;
    public LayerMask boidMask;

    public float attackRange;
    public float attackCD;
    public AudioClip attackSFX;
    public AudioClip swingSFX;

    //AI
    FSM<GoblinStatesEnum> _fsm;
    ITreeNode _root;
    public ISteering Steering => _steering;
    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
    ISteering _steering;
    bool _isAlone;
    [SerializeField] int minBoids;
    [SerializeField] float boidDetectionRadius;
    public bool atacking;

    private void Awake()
    {
        _los = GetComponent<LineOfSight>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        InitializeFSM(); 
        InitializeTree(); 
        InitializeSteerings();
    }

    void InitializeSteerings()
    {
        _obs = new ObstacleAvoidance(transform, angle, radius, obsMask, personalArea);
        _steering = GetComponent<FlockingManager>();
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
        _isAlone = Physics.OverlapSphere(transform.position, boidDetectionRadius, boidMask).Count() < minBoids;
    }

    void InitializeFSM()
    {
        //Declarating states
        var idle = new GoblinStateIdle<GoblinStatesEnum>(this);
        var chase = new GoblinStateChase<GoblinStatesEnum>(this);
        var evade = new GoblinStateEvade<GoblinStatesEnum>(this);
        var attack = new GoblinStateAttack<GoblinStatesEnum>(this);

        //Create Finite State Machine
        _fsm = new FSM<GoblinStatesEnum>(idle);

        //Creating transition between states
        idle.AddTransition(GoblinStatesEnum.Chase, chase);
        idle.AddTransition(GoblinStatesEnum.Evade, evade);
        idle.AddTransition(GoblinStatesEnum.Attack, attack);
        chase.AddTransition(GoblinStatesEnum.Idle, idle);
        chase.AddTransition(GoblinStatesEnum.Evade, evade);
        chase.AddTransition(GoblinStatesEnum.Attack, attack);
        evade.AddTransition(GoblinStatesEnum.Chase, chase);
        evade.AddTransition(GoblinStatesEnum.Idle, idle);
        evade.AddTransition(GoblinStatesEnum.Attack, attack);
        attack.AddTransition(GoblinStatesEnum.Chase, chase);
        attack.AddTransition(GoblinStatesEnum.Idle, idle);
        attack.AddTransition(GoblinStatesEnum.Evade, evade);
    }

    void InitializeTree()
    {
        //Actions
        ITreeNode idle = new ActionNode(() => _fsm.Transition(GoblinStatesEnum.Idle));
        ITreeNode flockedChase = new ActionNode(() => _fsm.Transition(GoblinStatesEnum.Chase));
        ITreeNode evade = new ActionNode(() => _fsm.Transition(GoblinStatesEnum.Evade));
        ITreeNode attack = new ActionNode(() => _fsm.Transition(GoblinStatesEnum.Attack));

        //Questions
        ITreeNode qEvade = new QuestionNode(QEvade, evade, idle);
        ITreeNode qChase = new QuestionNode(QChase, flockedChase, qEvade);
        ITreeNode qAttack = new QuestionNode(QAttack, attack, qChase);

        //First node to execute
        _root = qAttack;
    }

    public bool QChase() => !_isAlone && !_los.HasLOS(_attackRange) && _los.HasLOS(_los.Vision);
    public bool QAttack() => !_isAlone && _los.HasLOS(_attackRange);
    public bool QEvade() => _isAlone && _los.HasLOS(_los.Vision);

    public void Move(Vector3 dirToMove)                    //Move to the wished direction
    {
        dirToMove *= Speed;
        dirToMove.y = RB.velocity.y;
        RB.velocity = dirToMove;
    }

    public void LookDir(Vector3 dirToLook)                 //Rotate to the wished direction
    {
        if (dirToLook.x == 0 && dirToLook.z == 0) return;
        transform.forward = dirToLook;
    }
}
