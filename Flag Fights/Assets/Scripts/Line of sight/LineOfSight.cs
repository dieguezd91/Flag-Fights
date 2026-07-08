using UnityEngine;

public class LineOfSight : MonoBehaviour, ILineOfSight
{
    public float Vision => _vision;
    [SerializeField] float _vision;
    [Range(1, 360)] [SerializeField] float angle;
    [SerializeField] public LayerMask obstacles;

    Transform _target;
    public Transform TargetLOS => _target;

    Vector3 directionToTarget;
    Vector3 Origin => transform.position;
    Vector3 Forward => transform.forward;

    public void SetTarget(Transform target) => _target = target;

    public bool HasLOS() => _target != null
        && !IsTargetDead()
        && CheckRange(_target.position)
        && CheckAngle(_target.position)
        && CheckObstacles(_target.position);

    public bool HasLOS(float range) => _target != null
        && !IsTargetDead()
        && CheckRange(_target.position, range)
        && CheckAngle(_target.position)
        && CheckObstacles(_target.position);

    public bool HasLOS(float range, Vector3 target)
    {
        if (IsTargetDead()) return false;
        return CheckRange(target, range) && CheckAngle(target) && CheckObstacles(target);
    }

    private bool IsTargetDead()
    {
        if (_target == null) return false;
        var model = _target.GetComponent<PlayerModel>();
        return model != null && model.IsDead;
    }

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
