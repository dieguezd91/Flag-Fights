using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sword : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip swingSFX;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(attackSFX);
            GameManager.instance.EndRound(false);
        }
        else audioSource.PlayOneShot(swingSFX);
    }
}
