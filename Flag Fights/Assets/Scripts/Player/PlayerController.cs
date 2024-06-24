using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _model;
    private PlayerView _view;

    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;

    //FSM
    private FSM<PlayerStatesEnum> _fsm;
    private ITreeNode _root;

    private void Awake()
    {
        _model = new PlayerModel(_speed, _turnSpeed);
        _view = GetComponent<PlayerView>();
        InitializeFSM();
        InitializeTree();
    }

    void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
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