using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateAttack<T> : State<T>
{
    private EnemyController controller;
    private float lastAttackTime;

    public EnemyStateAttack(EnemyController controller)
    {
        this.controller = controller;
    }

    public override void Execute()
    {
        base.Execute();
        TryAttack();
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= controller.Model.AttackCD)
        {
            controller.View.PlayAttackAnimation();
            lastAttackTime = Time.time;
            Collider[] collidersAhead = Physics.OverlapSphere(controller.View.transform.position + controller.View.transform.forward * 0.35f + controller.View.transform.up * 0.5f, 0.4f);
            foreach (Collider col in collidersAhead)
            {
                if (col.CompareTag("Player"))
                {
                    controller.View.PlaySound(controller.Model.AttackSFX);
                    GameManager.instance.EndRound(false);
                }
                else
                {
                    controller.View.PlaySound(controller.Model.SwingSFX);
                }
            }
        }
    }
}

