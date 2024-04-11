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

    FSM<EnemyStatesEnum> _fsm;

    private void Awake()
    {
        InitializeFSM();    
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
        Debug.Log(_lineOfSight);
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
}
