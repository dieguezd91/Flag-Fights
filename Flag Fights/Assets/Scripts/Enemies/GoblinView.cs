using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinView : MonoBehaviour
{
    GoblinController goblinController;

    //COMPONENTS
    public Animator _animator;
    public Rigidbody RB { get; set; }
    public LineOfSight LOS { get; set; }
    public ObstacleAvoidance OBS { get; set; }
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
