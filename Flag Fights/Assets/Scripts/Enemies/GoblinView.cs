using UnityEngine;

public class GoblinView : MonoBehaviour, IView
{
    GoblinController goblinController;

    [HideInInspector] public Animator _animator;
    [HideInInspector] public Rigidbody RB;
    [HideInInspector] public LineOfSight LOS;
    [HideInInspector] public ObstacleAvoidance OBS;

    private void Awake()
    {
        LOS = GetComponent<LineOfSight>();
        _animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void PlayAttackAnimation()
    {
        if (_animator != null)
            _animator.SetTrigger("Attack");
    }

    public void Move(Vector3 direction, float speed)
    {
        direction *= speed;
        direction.y = RB.velocity.y;
        RB.velocity = direction;
    }

    public void LookDir(Vector3 direction)
    {
        direction.y = 0;
        if (direction.x != 0 || direction.z != 0)
        {
            transform.forward = direction;
        }
    }
}
