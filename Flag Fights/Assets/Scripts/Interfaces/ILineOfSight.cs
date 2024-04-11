using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILineOfSight
{
    bool CheckRange(Transform target);
    bool CheckRange(Transform target, float range);
    bool CheckAngle(Transform target);
    bool CheckObstacles(Transform target);

    bool HasLineOfSight();
    bool HasLineOfSight(float range);
}
