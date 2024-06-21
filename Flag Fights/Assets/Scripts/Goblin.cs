using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Goblin : MonoBehaviour
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
    public float Speed;
    public float angle;
    public float radius;
    public float personalArea;
    public LayerMask obsMask;

    //AI
    FSM<GoblinStatesEnum> _fsm;
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
        Debug.Log(_fsm.CurrentState);
    }

    void InitializeFSM()
    {
        //Declarating states
        var idle = new GoblinStateIdle<GoblinStatesEnum>(this);
        var evade = new GoblinStateEvade<GoblinStatesEnum>(this);

        //Create Finite State Machine
        _fsm = new FSM<GoblinStatesEnum>(idle);

        //Creating transition between states
        idle.AddTransition(GoblinStatesEnum.Evade, evade);
        evade.AddTransition(GoblinStatesEnum.Idle, idle);
    }

    void InitializeTree()
    {
        //Actions
        ITreeNode idle = new ActionNode(() => _fsm.Transition(GoblinStatesEnum.Idle));
        ITreeNode evade = new ActionNode(() => _fsm.Transition(GoblinStatesEnum.Evade));

        //Questions
        ITreeNode qEvade = new QuestionNode(QEvade, evade, idle);

        //First node to execute
        _root = qEvade;
    }

    public bool QEvade() => _los.HasLOS(_los.Vision);

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
