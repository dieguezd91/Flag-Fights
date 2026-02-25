using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GoblinStateAttack<T> : State<T>
{
    GoblinController _controller;
    GoblinModel _model;
    GoblinView _view;
    float _lastAttackTime;

    public GoblinStateAttack(GoblinController controller, GoblinModel model, GoblinView view)
    {
        _controller = controller;
        _model = model;
        _view = view;
    }

    public override void Execute()
    {
        Vector3 dirToTarget = _view.LOS.TargetLOS.position - _controller.transform.position;
        dirToTarget.y = 0;
        Vector3 dir = dirToTarget.normalized;
        _view.LookDir(dir);
        if (Time.time - _lastAttackTime >= _model.attackCD)               //Attack the player
        {
            _view._animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
            Collider[] collidersAhead = Physics.OverlapSphere(_controller.transform.position + _controller.transform.forward * .25f + _controller.transform.up * .25f, .2f);
            foreach (Collider col in collidersAhead)
            {
                if (col.CompareTag("Player"))
                {
                    _view.AudioSource.PlayOneShot(_model.attackSFX);
                    GameManager.instance.EndRound(false);
                }
                else _view.AudioSource.PlayOneShot(_model.swingSFX);
            }
        }
    }
}
