using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody _rb;
    public bool hasLineOfSight = false;
    public Vector2 targetDirection;
    public Vector2 directionToPlayer;
    protected Transform target;
    protected LineOfSight _lineOfSight;
    public float speed;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _lineOfSight = GetComponent<LineOfSight>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (hasLineOfSight) 
        {
            transform.Translate(_lineOfSight.directionToTarget * speed * Time.deltaTime);
        }
    }
}
