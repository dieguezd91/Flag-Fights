using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateAttack<T> : State<T>
{
    Enemy _enemy;
    float _lastAttackTime;

    public EnemyStateAttack(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void Execute()
    {
        base.Execute();
        TryAttack();
    }

    private void TryAttack()
    {
        if (Time.time - _lastAttackTime >= _enemy.attackCD)               //Attack the player
        {
            _enemy.Animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
            Collider[] collidersAhead = Physics.OverlapSphere(_enemy.transform.position + _enemy.transform.forward * .35f + _enemy.transform.up * .5f, 0.4f);
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
