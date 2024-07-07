using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;
    public Rigidbody RB { get; private set; }
    public LineOfSight LineOfSight { get; private set; }
    public ObstacleAvoidance ObstacleAvoidance { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public AgentController AgentController { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        LineOfSight = GetComponent<LineOfSight>();
        ObstacleAvoidance = new ObstacleAvoidance(transform, 90f, 10f, LayerMask.GetMask("Obstacles"), 2f);
        AudioSource = GetComponent<AudioSource>();
        AgentController = GetComponent<AgentController>();
    }

    public void PlayAttackAnimation()
    {
        Animator.SetTrigger("Attack");
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource.PlayOneShot(clip);
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
}