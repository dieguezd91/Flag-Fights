using UnityEngine;

public class PlayerStateRun<T> : State<T>
{
    private PlayerView _view;
    private PlayerModel _model;
    private Transform _transform;

    public PlayerStateRun(PlayerView view, PlayerModel model, Transform transform)
    {
        _view = view;
        _model = model;
        _transform = transform;
    }

    public override void Enter()
    {
        if (_view.Animator != null)
        {
            _view.Animator.SetBool("Running", true);
        }
    }

    public override void Execute()
    {
        if (_model != null && _model.IsDead) return;

        if (_model != null && _model.CurrentMoveDirection.sqrMagnitude > 0.001f)
        {
            Vector3 desiredDirection = _model.CurrentMoveDirection;
            desiredDirection.y = 0f;
            desiredDirection.Normalize();

            // Rotate towards desiredDirection using existing turn speed (degrees/sec)
            Quaternion targetRot = Quaternion.LookRotation(desiredDirection);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRot, _model.TurnSpeed * Time.deltaTime);

            // Calculate actual move direction based on actual body forward
            Vector3 actualMoveDirection = _transform.forward;
            actualMoveDirection.y = 0f;
            actualMoveDirection.Normalize();

            // Calculate alignment
            float alignment = Vector3.Dot(actualMoveDirection, desiredDirection);
            float alignmentSpeedMultiplier = Mathf.InverseLerp(0.2f, 0.95f, alignment);

            // Move along actual body forward
            _transform.Translate(actualMoveDirection * Time.deltaTime * _model.Speed * _model.CurrentMoveInput * alignmentSpeedMultiplier, Space.World);
        }
    }

    public override void Sleep()
    {
        if (_view.Animator != null)
        {
            _view.Animator.SetBool("Running", false);
        }
    }
}