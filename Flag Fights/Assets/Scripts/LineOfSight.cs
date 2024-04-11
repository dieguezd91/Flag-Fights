using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour, ILineOfSight
{
    public float range;
    [Range(1, 360)] public float angle;
    public LayerMask obstacles; //Capa de obstaculos de vision
    public Transform TargetLOS; //Posicion del jugador
    public Vector3 directionToTarget;


    protected virtual void Start()
    {
        //Obtener el transform del jugador
        TargetLOS = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        HasLineOfSight();
    }

    public bool HasLineOfSight()
    {
        //Chequear que se cumplan todas las condiciones de deteccion en la linea de vision del enemigo
        return CheckRange(TargetLOS) && CheckAngle(TargetLOS) && CheckObstacles(TargetLOS);
    }

    //Chequea la distancia entre el enemigo y el jugador
    public bool CheckRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, Origin);
        //Debug.Log(distance + " es la distancia");
        //Debug.Log(distance <= _enemy.enemyData.RangeDetection);
        return distance <= range;

    }

    //Obtiene la direccion desde el enemigo hasta el jugador
    public bool CheckAngle(Transform target)
    {
        directionToTarget = target.position - Origin;
        //Debug.Log(directionToTarget + " es la direccion al objetivo");
        float angleToTarget = Vector3.Angle(Forward, directionToTarget);
        //Debug.Log(angleToTarget <= _enemy.enemyData.AngleDetection / 2);
        return angleToTarget <= angle / 2;
    }

    //Chequea que no haya obstaculos visuales entre el enemigo y su objetivo
    public bool CheckObstacles(Transform target)
    {
        directionToTarget = target.position - Origin;
        float distance = directionToTarget.magnitude;
        //Debug.Log(distance);
        //Debug.DrawRay(Origin, directionToTarget, Color.green, obstacles);        

        return !Physics2D.Raycast(Origin, directionToTarget, distance, obstacles);
    }

    //posision del enemigo
    Vector3 Origin => transform.position;
    //lado de vision del enemigo
    public Vector3 Forward => transform.forward;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Origin, range);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * range);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -(angle / 2), 0) * Forward * range);
    }
}
