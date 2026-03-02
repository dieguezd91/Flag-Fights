using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _model;
    private PlayerView _view;

    // FSM
    private FSM<PlayerStatesEnum> _fsm;
    private ITreeNode _root;

    private void Awake()
    {
        _model = GetComponent<PlayerModel>();
        _view = GetComponent<PlayerView>();

        if (_model == null || _view == null)
        {
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
        _model.MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Flag"))
        {
            collision.gameObject.SetActive(false);
            _view.SetFlagVisibility(true);
            _model.HasFlag = true;
        }
    }

    public void ResetRoundState()
    {
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

    bool QRun() => _model.MovementInput != Vector2.zero;
}