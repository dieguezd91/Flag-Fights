using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public GameObject flag;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void SetFlagVisibility(bool isVisible)
    {
        flag.SetActive(isVisible);
    }
}
