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
        _transform.Translate(Vector3.forward * Time.deltaTime * _model.Speed * _model.CurrentMoveInput);
        _transform.Rotate(Vector3.up, _model.TurnSpeed * _model.TurnInput * Time.deltaTime);
    }

    public override void Sleep()
    {
        if (_view.Animator != null)
        {
            _view.Animator.SetBool("Running", false);
        }
    }
}