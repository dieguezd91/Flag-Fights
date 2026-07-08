using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public bool HasFlag = false;
    public bool IsDead = false;

    public float Speed;
    public float TurnSpeed;
    public float MoveInput;
    public float TurnInput;

    public float Acceleration = 4f;
    public float Deceleration = 8f;
    public float CurrentMoveInput;

    public Vector3 CurrentMoveDirection { get; private set; }

    public void SetMoveDirection(Vector3 direction)
    {
        CurrentMoveDirection = direction;
    }
}