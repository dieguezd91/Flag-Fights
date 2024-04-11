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

    //Stats
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;

    //FSM
    FSM<PlayerStatesEnum> _fsm;

    private void Awake() => InitializeFSM();

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
        var idle = new PlayerStateIdle<PlayerStatesEnum>(PlayerStatesEnum.Walk);
        var walk = new PlayerStateWalk<PlayerStatesEnum>(speed, turnSpeed, transform, PlayerStatesEnum.Idle);

        //Create Finite State Machine
        _fsm = new FSM<PlayerStatesEnum>(idle);

        //Create transitions between states
        idle.AddTransition(PlayerStatesEnum.Walk, walk);
        walk.AddTransition(PlayerStatesEnum.Idle, idle);
    }
}
