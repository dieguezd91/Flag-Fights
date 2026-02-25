using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier;
    public float predatorRange;
    Transform target;

    private void Awake()
    {
        target = GameManager.instance.player.transform;
    }


    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        if (GoblinController.CurrentLeader != null) return Vector3.zero;
        var diff = self.Position - target.position;
        if (diff.magnitude > predatorRange) return Vector3.zero;
        Vector3 predatorDir = diff.normalized * (predatorRange - diff.magnitude);
        return predatorDir * multiplier;
    }
}
