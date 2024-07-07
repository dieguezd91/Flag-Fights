using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinStateIdle<T> : State<T>
{
    GoblinController _controller;

    public GoblinStateIdle(GoblinController enemy)
    {
        _controller = enemy;
    }

    public override void Enter()
    {
        if (_controller == null)
        {
            Debug.LogError("_enemyController no está inicializado.");
            return;
        }
        if (_controller.View._animator == null)
        {
            Debug.LogError("_enemyController.View.Animator no está inicializado.");
            return;
        }

        _controller.View._animator.SetBool("Idle", true);
    }
    
    public override void Sleep()
    {
        _controller.View._animator.SetBool("Idle", false);
    }
}
