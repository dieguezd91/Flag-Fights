using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

public class GoblinController : MonoBehaviour, IBoid
{
    [HideInInspector] public GoblinModel Model;
    [HideInInspector] public GoblinView View;
    //AI
    FSM<GoblinStatesEnum> _fsm;
    ITreeNode _root;

    ISteering _steering;

    bool _isAlone;
    [SerializeField] int minBoids;
    [SerializeField] float boidDetectionRadius;

    public bool atacking;

    private void Awake()
    {
        Model = GetComponent<GoblinModel>();
        View = GetComponent<GoblinView>();
        InitializeFSM(); 
        InitializeTree(); 
        InitializeSteerings();
    }

    void InitializeSteerings()
    {
        View.OBS = new ObstacleAvoidance(transform, Model.angle, Model.radius, Model.obsMask, Model.personalArea);
        _steering = GetComponent<FlockingManager>();
    }

    private void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
        _isAlone = Physics.OverlapSphere(transform.position, boidDetectionRadius, Model.boidMask).Count() < minBoids;
    }

    void InitializeFSM()
    {
        //Declarating states
        var idle = new GoblinStateIdle<GoblinStatesEnum>(this, View, Model);
        var chase = new GoblinStateChase<GoblinStatesEnum>(this, View, Model);
        var evade = new GoblinStateEvade<GoblinStatesEnum>(this, View, Model);
        var attack = new GoblinStateAttack<GoblinStatesEnum>(this, View, Model);

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

    public bool QChase() => !_isAlone && !View.LOS.HasLOS(Model._attackRange) && View.LOS.HasLOS(View.LOS.Vision);
    public bool QAttack() => !_isAlone && View.LOS.HasLOS(Model._attackRange);
    public bool QEvade() => _isAlone && View.LOS.HasLOS(View.LOS.Vision);

    public ISteering Steering => _steering;
    public Vector3 Position => transform.position;
    public Vector3 Front => transform.forward;
}
