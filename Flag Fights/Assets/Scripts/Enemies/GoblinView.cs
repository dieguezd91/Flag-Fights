using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinView : MonoBehaviour, IView
{
    GoblinController goblinController;

    //COMPONENTS
    [HideInInspector] public Animator _animator;
    [HideInInspector] public Rigidbody RB;
    [HideInInspector] public LineOfSight LOS;
    [HideInInspector] public ObstacleAvoidance OBS;
    AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    private void Awake()
    {
        LOS = GetComponent<LineOfSight>();
        _animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");
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
