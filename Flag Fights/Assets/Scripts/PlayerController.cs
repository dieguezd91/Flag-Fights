using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool hasFlag = false;
    public GameObject flag;

    FSM<PlayerStatesEnum> _fsm;

    private void Awake()
    {
        InitializeFSM();
    }

    void Update()
    {
        _fsm.OnUpdate();
    }

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
        var idle = new PlayerStateIdle<PlayerStatesEnum>(PlayerStatesEnum.Walk);
        var walk = new PlayerStateWalk<PlayerStatesEnum>(transform, PlayerStatesEnum.Idle);

        _fsm = new FSM<PlayerStatesEnum>(idle);

        idle.AddTransition(PlayerStatesEnum.Walk, walk);
        walk.AddTransition(PlayerStatesEnum.Idle, idle);
    }
}
