using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GoblinStateAttack<T> : State<T>
{
    GoblinController _enemy;
    float _lastAttackTime;

    public GoblinStateAttack(GoblinController enemy)
    {
        _enemy = enemy;
    }

    public override void Execute()
    {
        Vector3 dir = (_enemy.View.LOS.TargetLOS.position - _enemy.transform.position).normalized;
        _enemy.View.LookDir(dir);
        if (Time.time - _lastAttackTime >= _enemy.Model.attackCD)               //Attack the player
        {
            _enemy.View._animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
            Collider[] collidersAhead = Physics.OverlapSphere(_enemy.transform.position + _enemy.transform.forward * .25f + _enemy.transform.up * .25f, .2f);
            foreach (Collider col in collidersAhead)
            {
                if (col.CompareTag("Player"))
                {
                    _enemy.View.AudioSource.PlayOneShot(_enemy.Model.attackSFX);
                    GameManager.instance.EndRound(false);
                }
                else _enemy.View.AudioSource.PlayOneShot(_enemy.Model.swingSFX);
            }
        }
    }
}
