using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateAttack<T> : State<T>
{
    KnightController controller;
    KnightView _view;
    KnightModel _model;
    private float lastAttackTime;

    public EnemyStateAttack(KnightController controller, KnightModel model, KnightView view)
    {
        this.controller = controller;
        _model = model;
        _view = view;
    }

    public override void Enter()
    {
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
        _view.PlayAttackAnimation();
        lastAttackTime = Time.time;
    }

    public override void Sleep()
    {
        _view._animator.ResetTrigger("Attack");
    }

    public override void Execute()
    {
        base.Execute();
        TryAttack();
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= _model.attackCD)
        {
            _view.PlayAttackAnimation();
            lastAttackTime = Time.time;
            Collider[] collidersAhead = Physics.OverlapSphere(_view.transform.position + _view.transform.forward * 0.35f + _view.transform.up * 0.5f, 0.4f);
            foreach (Collider col in collidersAhead)
            {
                if (col.CompareTag("Player"))
                {
                    AudioManager.Instance?.PlaySFX(_model.attackSFX);
                    GameManager.instance.EndRound(false);
                }
                else AudioManager.Instance?.PlaySFX(_model.swingSFX);
            }
        }
    }
}

