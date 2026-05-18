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
        _view.Animator.SetBool("Running", true);
    }

    public override void Execute()
    {
        _transform.Translate(Vector3.forward * Time.deltaTime * _model.Speed * _model.MoveInput);
        _transform.Rotate(Vector3.up, _model.TurnSpeed * _model.TurnInput * Time.deltaTime);
    }

    public override void Sleep()
    {
        _view.Animator.SetBool("Running", false);
    }
}