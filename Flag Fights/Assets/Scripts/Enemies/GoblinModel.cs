using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinModel : EnemyModel
{
    public LayerMask obsMask;
    public LayerMask boidMask;
    public AudioClip attackSFX;
    public AudioClip swingSFX;
}
