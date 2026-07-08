using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel _model;
    [SerializeField] private PlayerView _view;

    PlayerFlagCarryVisual _flagCarryVisual;

    // FSM
    private FSM<PlayerStatesEnum> _fsm;
    private ITreeNode _root;

    private void Awake()
    {
        if (_model == null) _model = GetComponent<PlayerModel>();
        if (_view == null) _view = GetComponent<PlayerView>();
        _flagCarryVisual = GetComponent<PlayerFlagCarryVisual>();

        if (_model == null || _view == null) return;

        InitializeFSM();
        InitializeTree();
    }

    void Update()
    {
        UpdateMovementInput();
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }

    private void UpdateMovementInput()
    {
        if (_model == null) return;

        _model.MoveInput = Input.GetAxisRaw("Vertical");
        _model.TurnInput = Input.GetAxisRaw("Horizontal");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Flag")) return;
        if (_model == null || _view == null) return;

        GameObject pickedFlag = collision.gameObject;
        GameEvents.RaiseFlagPickedUp(this, pickedFlag);

        pickedFlag.SetActive(false);
        _model.HasFlag = true;

        GameEvents.RaiseFlagCarryChanged(this, true);

        // TODO: Remove this fallback after all player prefabs require PlayerFlagCarryVisual.
        if (ShouldUseLegacyFlagVisibility())
            _view.SetFlagVisibility(true);
    }

    public void ResetRoundState()
    {
        if (_model == null || _view == null) return;

        bool hadFlag = _model.HasFlag;
        _model.HasFlag = false;

        if (hadFlag)
            GameEvents.RaiseFlagCarryChanged(this, false);

        // TODO: Remove this fallback after all player prefabs require PlayerFlagCarryVisual.
        if (ShouldUseLegacyFlagVisibility())
            _view.SetFlagVisibility(false);
    }

    bool ShouldUseLegacyFlagVisibility()
    {
        return _flagCarryVisual == null || !_flagCarryVisual.isActiveAndEnabled;
    }

    void InitializeFSM()
    {
        var idle = new PlayerStateIdle<PlayerStatesEnum>(_view, _model, PlayerStatesEnum.Run);
        var run = new PlayerStateRun<PlayerStatesEnum>(_view, _model, transform);
        _fsm = new FSM<PlayerStatesEnum>(idle);
        idle.AddTransition(PlayerStatesEnum.Run, run);
        run.AddTransition(PlayerStatesEnum.Idle, idle);
    }

    void InitializeTree()
    {
        ITreeNode idle = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Idle));
        ITreeNode running = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Run));
        ITreeNode qRun = new QuestionNode(QRun, running, idle);
        _root = qRun;
    }

    bool QRun() => _model != null && Mathf.Abs(_model.MoveInput) > 0.01f;
}
