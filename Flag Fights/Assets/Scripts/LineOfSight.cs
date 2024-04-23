using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour, ILineOfSight
{
    [SerializeField] float _vision;                 //Rango de vision
    [Range(1, 360)] [SerializeField] float angle;   //Angulo de vision
    [SerializeField] LayerMask obstacles;           //Capa de obstaculos de vision
    Transform _targetLOS;                           //Jugador
    public Transform TargetLOS => _targetLOS;
    Vector3 directionToTarget;                      //Direccion hacia el objetivo
    Vector3 Origin => transform.position;           //Posicion
    Vector3 Forward => transform.forward;

    //Obtener el transform del jugador
    protected virtual void Start() => _targetLOS = GameObject.FindGameObjectWithTag("Player").transform;


    //Chequear que se cumplan todas las condiciones de deteccion en la linea de vision del enemigo
    public bool HasLOS() => CheckRange(_targetLOS) && CheckAngle(_targetLOS); //&& CheckObstacles(_targetLOS);


    //Chequear que se cumplan todas las condiciones de deteccion en la linea de vision del enemigo
    public bool HasLOS(float range) => CheckRange(_targetLOS, range) && CheckAngle(_targetLOS) && CheckObstacles(_targetLOS);


    //Chequear que la distancia hacia el objetivo sea menor al rango de vision
    public bool CheckRange(Transform target)
    {
        float distance = Vector3.Distance(target.position, Origin);
        return distance <= _vision;
    }


    //Chequear que la distancia hacia el objetivo sea menor al parametro dado
    public bool CheckRange(Transform target, float range)
    {
        float distance = Vector3.Distance(target.position, Origin);
        return distance <= range;

    }


    //Chequear que la direccion hacia el objetivo este dentro del angulo de vision
    public bool CheckAngle(Transform target)
    {
        directionToTarget = target.position - Origin;
        float angleToTarget = Vector3.Angle(Forward, directionToTarget);
        return angleToTarget <= angle / 2;
    }


    //Chequear que no haya obstaculos entre el objetivo y self
    public bool CheckObstacles(Transform target)
    {
        directionToTarget = target.position - Origin;
        float distance = directionToTarget.magnitude;
        return !Physics.Raycast(Origin, directionToTarget, distance, obstacles);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Origin, _vision);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * _vision);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -(angle / 2), 0) * Forward * _vision);
    }
}
