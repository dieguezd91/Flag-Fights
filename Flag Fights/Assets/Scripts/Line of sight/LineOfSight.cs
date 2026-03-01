using UnityEngine;

public class LineOfSight : MonoBehaviour, ILineOfSight
{
    public float Vision => _vision;
    [SerializeField] float _vision;
    [Range(1, 360)] [SerializeField] float angle;
    [SerializeField] public LayerMask obstacles;
    public Transform TargetLOS => GameManager.instance.player.transform;
    Vector3 directionToTarget;
    Vector3 Origin => transform.position;
    Vector3 Forward => transform.forward;

    public bool HasLOS() => CheckRange(GameManager.instance.player.transform.position) && CheckAngle(GameManager.instance.player.transform.position) && CheckObstacles(GameManager.instance.player.transform.position);

    public bool HasLOS(float range) => CheckRange(GameManager.instance.player.transform.position, range) && CheckAngle(GameManager.instance.player.transform.position) && CheckObstacles(GameManager.instance.player.transform.position);
    public bool HasLOS(float range, Vector3 target) => CheckRange(target, range) && CheckAngle(target) && CheckObstacles(target);

    public bool CheckRange(Vector3 target)
    {
        float distance = Vector3.Distance(target, Origin);
        return distance <= _vision;
    }

    public bool CheckRange(Vector3 target, float range)
    {
        float distance = Vector3.Distance(target, Origin);
        return distance <= range;

    }

    public bool CheckAngle(Vector3 target)
    {
        directionToTarget = target - Origin;
        float angleToTarget = Vector3.Angle(Forward, directionToTarget);
        return angleToTarget <= angle / 2;
    }

    public bool CheckObstacles(Vector3 target)
    {
        directionToTarget = target - Origin;
        float distance = directionToTarget.magnitude;
        return !Physics.Raycast(Origin, directionToTarget, distance, obstacles);
    }
}
