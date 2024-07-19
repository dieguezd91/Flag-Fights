using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public GameObject flag;

    private Renderer[] renderers;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        renderers = GetComponentsInChildren<Renderer>();

        if (Animator == null)
        {
            Debug.Log("Animator null");
        }

        if (flag == null)
        {
            Debug.Log("Flag null");
        }

        if (renderers == null || renderers.Length == 0)
        {
            Debug.Log("No renderers found");
        }
    }

    public void SetFlagVisibility(bool isVisible)
    {
        if (flag != null)
        {
            flag.SetActive(isVisible);
        }
    }

    public void SetVisibility(bool isVisible)
    {
        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = isVisible;
            }
        }
    }
}

