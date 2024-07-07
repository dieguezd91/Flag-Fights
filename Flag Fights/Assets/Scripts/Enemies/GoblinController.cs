using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

public class GoblinController : EnemyController, IBoid
{
    [HideInInspector] public GoblinModel Model;
    [HideInInspector] public GoblinView View;

    //AI
    FSM<GoblinStatesEnum> _fsm;
    bool _isAlone;
    [SerializeField] int minBoids;
    [SerializeField] float boidDetectionRadius;

    public bool atacking;

    public override void Awake()
    {
        Model = GetComponent<GoblinModel>();
        View = GetComponent<GoblinView>();
        InitializeFSM(); 
        InitializeTree(); 
        InitializeSteerings();
    }

    public override void InitializeSteerings()
    {
        View.OBS = new ObstacleAvoidance(transform, Model.angle, Model.radius, Model.obsMask, Model.personalArea);
        _steering = GetComponent<FlockingManager>();
    }

    public override void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
        _isAlone = Physics.OverlapSphere(transform.position, boidDetectionRadius, Model.boidMask).Count() < minBoids;
    }

    public override void InitializeFSM()
    {
        //Declarating states
        var idle = new GoblinStateIdle<GoblinStatesEnum>(this, Model, View);
        var chase = new GoblinStateChase<GoblinStatesEnum>(this, Model, View);
        var evade = new GoblinStateEvade<GoblinStatesEnum>(this, Model, View);
        var attack = new GoblinStateAttack<GoblinStatesEnum>(this, Model, View);

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

    public override void InitializeTree()
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

    public bool QChase() => !_isAlone && !View.LOS.HasLOS(Model.attackRange) && View.LOS.HasLOS(View.LOS.Vision);
    public bool QAttack() => !_isAlone && View.LOS.HasLOS(Model.attackRange);
    public bool QEvade() => _isAlone && View.LOS.HasLOS(View.LOS.Vision);

    public ISteering Steering => _steering;
    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
}
