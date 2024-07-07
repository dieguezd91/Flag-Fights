using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILineOfSight
{
    bool CheckRange(Vector3 target);
    bool CheckRange(Vector3 target, float range);
    bool CheckAngle(Vector3 target);
    bool CheckObstacles(Vector3 target);

    bool HasLOS();
    bool HasLOS(float range);
}
