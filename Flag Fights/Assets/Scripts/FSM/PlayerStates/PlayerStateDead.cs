public class PlayerStateDead<T> : State<T>
{
    private readonly PlayerView _view;
    private readonly PlayerModel _model;

    public PlayerStateDead(PlayerView view, PlayerModel model)
    {
        _view = view;
        _model = model;
    }

    public override void Enter()
    {
        _model.MoveInput = 0f;
        _model.TurnInput = 0f;
        _model.CurrentMoveInput = 0f;

        _view.PlayDeath();
    }

    public override void Execute()
    {
        // Death is terminal until round reset.
    }

    public override void Sleep()
    {
        _view.ClearDeath();
    }
}