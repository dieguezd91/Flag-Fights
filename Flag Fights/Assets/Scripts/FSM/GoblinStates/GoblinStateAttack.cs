using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GoblinStateAttack<T> : State<T>
{
    Goblin _enemy;
    float _lastAttackTime;

    public GoblinStateAttack(Goblin enemy)
    {
        _enemy = enemy;
    }

    public override void Execute()
    {
        Vector3 dir = (_enemy.LOS.TargetLOS.position - _enemy.transform.position).normalized;
        _enemy.LookDir(dir);
        if (Time.time - _lastAttackTime >= _enemy.attackCD)               //Attack the player
        {
            _enemy.Animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
            Collider[] collidersAhead = Physics.OverlapSphere(_enemy.transform.position + _enemy.transform.forward * .25f + _enemy.transform.up * .25f, .2f);
            foreach (Collider col in collidersAhead)
            {
                if (col.tag == "Player")
                {
                    _enemy.AudioSource.PlayOneShot(_enemy.attackSFX);
                    GameManager.instance.EndRound(false);
                }
                else _enemy.AudioSource.PlayOneShot(_enemy.swingSFX);
            }
        }
    }
}
