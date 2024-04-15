using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Flag management
    public bool hasFlag = false;
    public GameObject flag;

    Animator _animator;

    //Stats
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;

    //FSM
    FSM<PlayerStatesEnum> _fsm;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        InitializeFSM();
    }

    void Update() => _fsm.OnUpdate();

        //Check collision with flag
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Flag"))
        {
            collision.gameObject.SetActive(false);
            flag.SetActive(true);
            hasFlag = true;
        }
    }

    void InitializeFSM()
    {
        //States declarations
        var idle = new PlayerStateIdle<PlayerStatesEnum>(_animator, speed, PlayerStatesEnum.Run);
        var run = new PlayerStateRun<PlayerStatesEnum>(_animator, speed, turnSpeed, transform, PlayerStatesEnum.Idle);

        //Create Finite State Machine
        _fsm = new FSM<PlayerStatesEnum>(idle);

        //Create transitions between states
        idle.AddTransition(PlayerStatesEnum.Run, run);
        run.AddTransition(PlayerStatesEnum.Idle, idle);
    }
}
