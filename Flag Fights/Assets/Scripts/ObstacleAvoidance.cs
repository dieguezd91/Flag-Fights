using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleAvoidance
{
    float _angle;
    float _radius;
    float _personalArea;
    Transform _entity;
    LayerMask _maskObs;

    public ObstacleAvoidance(Transform entity, float angle, float radius, LayerMask maskObs, float personalArea)
    {
        _angle = angle;
        _radius = radius;
        _entity = entity;
        _maskObs = maskObs;
        _personalArea = personalArea;
    }

    public Vector3 GetNewDir(Vector3 currentDir, bool calculateY = true)
    {
        Collider[] colls = Physics.OverlapSphere(_entity.position, _radius, _maskObs);
        Collider nearColl = null;
        Vector3 closestPoint = Vector3.zero;
        float nearCollDistance = 0;
        if (!calculateY) currentDir.y = 0;
        for (int i = 0; i < colls.Length; i++)
        {
            var currentColl = colls[i];
            closestPoint = currentColl.ClosestPoint(_entity.position);
            if (!calculateY) closestPoint.y = _entity.position.y;
            Vector3 dirToColl = closestPoint - _entity.position;
            float currentAngle = Vector3.Angle(dirToColl, currentDir);
            float distance = dirToColl.magnitude;

            if (currentAngle > _angle / 2) continue;

            if (nearColl == null)
            {
                nearColl = currentColl;
                nearCollDistance = distance;
                continue;
            }

            if (distance < nearCollDistance)
            {
                nearCollDistance = distance;
                nearColl = currentColl;
            }
            Debug.Log(nearColl);
            Debug.DrawLine(_entity.position, closestPoint);
        }
        if (nearColl == null)
        {
            Debug.Log(currentDir);
            return currentDir;
        }
        else
        {
            Vector3 relativePos = _entity.InverseTransformPoint(closestPoint);
            Vector3 dirToClosestPoint = (closestPoint - _entity.position).normalized;
            Vector3 newDir;
            if (relativePos.x < 0) newDir = Vector3.Cross(_entity.up, dirToClosestPoint);
            else newDir = -Vector3.Cross(_entity.up, dirToClosestPoint);
            return Vector3.Lerp(currentDir, newDir, (_radius - Mathf.Clamp(nearCollDistance - _personalArea, 0, _radius)) / _radius);
        }
    }
}
