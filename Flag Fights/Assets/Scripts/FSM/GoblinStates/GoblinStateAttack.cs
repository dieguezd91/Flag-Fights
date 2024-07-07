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

    public GoblinStateAttack(GoblinController controller, GoblinView view, GoblinModel model)
    {
        _controller = controller;
        _view = view;
        _model = model;
    }

    public override void Execute()
    {
        Vector3 dir = (_view.LOS.TargetLOS.position - _controller.transform.position).normalized;
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
