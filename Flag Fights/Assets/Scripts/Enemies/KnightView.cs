using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightView : MonoBehaviour, IView
{
    KnightController knightController;
    public Animator _animator;
    private Rigidbody _rb;
    public Rigidbody RB => _rb;
    private LineOfSight _los;
    public LineOfSight LineOfSight => _los;
    private ObstacleAvoidance _obs;
    public ObstacleAvoidance ObstacleAvoidance => _obs;
    private AgentController _agentController;
    public AgentController AgentController => _agentController;

    private void Awake()
    {
        knightController = GetComponent<KnightController>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _los = GetComponent<LineOfSight>();
        _agentController = GetComponent<AgentController>();
    }

    private void Start()
    {
        _obs = new ObstacleAvoidance(transform, knightController.Model.angle, knightController.Model.radius, _los.obstacles, knightController.Model.personalArea);
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
        if (direction.x != 0 || direction.z != 0)
        {
            transform.forward = direction;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, knightController.Model.radius);        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, knightController.Model.personalArea);
    }
}