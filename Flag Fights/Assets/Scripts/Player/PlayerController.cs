using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel _model;
    [SerializeField] private PlayerView _view;

    // FSM
    private FSM<PlayerStatesEnum> _fsm;
    private ITreeNode _root;

    private void Awake()
    {
        if (_model == null) _model = GetComponent<PlayerModel>();
        if (_view == null) _view = GetComponent<PlayerView>();

        if (_model == null || _view == null)
        {
            Debug.LogError($"[{nameof(PlayerController)}] Missing PlayerModel or PlayerView on {name}. Flag pickup is disabled.", this);
            return;
        }

        InitializeFSM();
        InitializeTree();
    }

    void Update()
    {
        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
        UpdateMovementInput();
    }

    private void UpdateMovementInput()
    {
        if (_model == null) return;

        _model.MoveInput = Input.GetAxisRaw("Vertical");
        _model.TurnInput = Input.GetAxisRaw("Horizontal");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Flag"))
        {
            return;
        }

        Debug.Log($"[{nameof(PlayerController)}] Flag contact detected with {collision.name}. Hand flag assigned: {_view != null && _view.HasFlagReference}", this);

        if (_model == null || _view == null)
        {
            Debug.LogError($"[{nameof(PlayerController)}] Cannot pick up flag because PlayerModel or PlayerView is missing on {name}.", this);
            return;
        }

        collision.gameObject.SetActive(false);
        _view.SetFlagVisibility(true);
        _model.HasFlag = true;
    }

    public void ResetRoundState()
    {
        if (_model == null || _view == null) return;

        _model.HasFlag = false;
        _view.SetFlagVisibility(false);
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