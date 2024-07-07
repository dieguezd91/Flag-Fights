using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinModel : MonoBehaviour
{
    //STATS
    public float _attackRange;
    public float Speed;
    public float angle;
    public float radius;
    public float personalArea;
    public LayerMask obsMask;
    public LayerMask boidMask;

    public float attackRange;
    public float attackCD;
    public AudioClip attackSFX;
    public AudioClip swingSFX;
}
