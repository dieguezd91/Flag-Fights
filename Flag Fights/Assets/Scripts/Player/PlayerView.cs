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

        if (Animator == null)
        {
            Debug.Log("Animator null");
        }

        if (flag == null)
        {
            Debug.Log("Flag null");
        }
    }

    public void SetFlagVisibility(bool isVisible)
    {
        if (flag != null)
        {
            flag.SetActive(isVisible);
        }
    }
}
