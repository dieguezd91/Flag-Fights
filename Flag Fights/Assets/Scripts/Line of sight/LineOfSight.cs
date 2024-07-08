using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour, ILineOfSight
{
    public float Vision => _vision;
    [SerializeField] float _vision;                 //Rango de vision
    [Range(1, 360)] [SerializeField] float angle;   //Angulo de vision
    [SerializeField] public LayerMask obstacles;           //Capa de obstaculos de vision
    Transform _targetLOS;                           //Jugador
    public Transform TargetLOS => GameManager.instance.player.transform;
    Vector3 directionToTarget;                      //Direccion hacia el objetivo
    Vector3 Origin => transform.position;           //Posicion
    Vector3 Forward => transform.forward;


    //Chequear que se cumplan todas las condiciones de deteccion en la linea de vision del enemigo
    public bool HasLOS() => CheckRange(GameManager.instance.player.transform.position) && CheckAngle(GameManager.instance.player.transform.position) && CheckObstacles(GameManager.instance.player.transform.position);


    //Chequear que se cumplan todas las condiciones de deteccion en la linea de vision del enemigo
    public bool HasLOS(float range) => CheckRange(GameManager.instance.player.transform.position, range) && CheckAngle(GameManager.instance.player.transform.position) && CheckObstacles(GameManager.instance.player.transform.position);
    public bool HasLOS(float range, Vector3 target) => CheckRange(target, range) && CheckAngle(target) && CheckObstacles(target);


    //Chequear que la distancia hacia el objetivo sea menor al rango de vision
    public bool CheckRange(Vector3 target)
    {
        float distance = Vector3.Distance(target, Origin);
        return distance <= _vision;
    }


    //Chequear que la distancia hacia el objetivo sea menor al parametro dado
    public bool CheckRange(Vector3 target, float range)
    {
        float distance = Vector3.Distance(target, Origin);
        return distance <= range;

    }


    //Chequear que la direccion hacia el objetivo este dentro del angulo de vision
    public bool CheckAngle(Vector3 target)
    {
        directionToTarget = target - Origin;
        float angleToTarget = Vector3.Angle(Forward, directionToTarget);
        return angleToTarget <= angle / 2;
    }


    //Chequear que no haya obstaculos entre el objetivo y self
    public bool CheckObstacles(Vector3 target)
    {
        directionToTarget = target - Origin;
        float distance = directionToTarget.magnitude;
        return !Physics.Raycast(Origin, directionToTarget, distance, obstacles);
    }
}
