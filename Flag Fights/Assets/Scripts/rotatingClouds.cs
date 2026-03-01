using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingClouds : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0));
    }
}
