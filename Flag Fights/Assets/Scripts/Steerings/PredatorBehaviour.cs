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
        Vector3 predatorDir = Vector3.zero;
        var diff = self.Position - target.position;
        predatorDir += diff.normalized * (predatorRange - diff.magnitude);
        return predatorDir.normalized * multiplier;
    }
}
