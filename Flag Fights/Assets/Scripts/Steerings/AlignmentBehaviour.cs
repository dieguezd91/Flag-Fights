using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier;
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            dir += boids[i].Front;
        }
        return dir.normalized * multiplier;
    }
}
