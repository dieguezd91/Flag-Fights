using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier;
    public Transform target;
    public bool isActive;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 dir = (target.position - self.Position).normalized * multiplier;
        Debug.DrawRay(self.Position, dir);
        return dir;
    }
}
