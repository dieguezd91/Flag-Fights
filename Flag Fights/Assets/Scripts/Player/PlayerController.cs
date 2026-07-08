using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerModel _model;
    [SerializeField] private PlayerView _view;

    PlayerFlagCarryVisual _flagCarryVisual;

    // FSM
    private FSM<PlayerStatesEnum> _fsm;
    private ITreeNode _root;

    private Transform _camTransform;

    private void Awake()
    {
        if (_model == null) _model = GetComponent<PlayerModel>();
        if (_view == null) _view = GetComponent<PlayerView>();
        _flagCarryVisual = GetComponent<PlayerFlagCarryVisual>();
        if (Camera.main != null) _camTransform = Camera.main.transform;

        if (_model == null || _view == null) return;

        InitializeFSM();
        InitializeTree();
    }

    void Update()
    {
        if (_model != null && _model.IsDead)
        {
            _fsm?.OnUpdate();
            return;
        }

        UpdateMovementInput();

        if (_fsm != null) _fsm.OnUpdate();
        if (_root != null) _root.Execute();
    }

    private void UpdateMovementInput()
    {
        if (_model == null) return;

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 rawInput = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(rawInput.magnitude);

        Vector3 moveDirection = Vector3.zero;
        if (_camTransform != null)
        {
            Vector3 camForward = _camTransform.forward;
            Vector3 camRight = _camTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = camForward * rawInput.y + camRight * rawInput.x;
            if (moveDirection.sqrMagnitude > 0.001f)
            {
                moveDirection.Normalize();
            }
            else
            {
                moveDirection = Vector3.zero;
            }
        }
        else
        {
            moveDirection = new Vector3(rawInput.x, 0f, rawInput.y);
            if (moveDirection.sqrMagnitude > 0.001f)
            {
                moveDirection.Normalize();
            }
            else
            {
                moveDirection = Vector3.zero;
            }
        }

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            _model.SetMoveDirection(moveDirection);
        }
        _model.MoveInput = inputMagnitude;
        _model.TurnInput = horizontalInput;

        float target = _model.MoveInput;
        float rate = Mathf.Abs(target) > Mathf.Abs(_model.CurrentMoveInput) ? _model.Acceleration : _model.Deceleration;
        _model.CurrentMoveInput = Mathf.MoveTowards(_model.CurrentMoveInput, target, rate * Time.deltaTime);
    }

    private void OnEnable()
    {
        GameEvents.OnRoundEnded += OnRoundEndedHandler;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_model == null || _view == null) return;
        if (_model.IsDead) return;
        if (GameManager.instance != null && GameManager.instance.IsRoundEnding) return;

        if (!collision.CompareTag("Flag")) return;

        GameObject pickedFlag = collision.gameObject;
        GameEvents.RaiseFlagPickedUp(this, pickedFlag);

        pickedFlag.SetActive(false);
        _model.HasFlag = true;

        GameEvents.RaiseFlagCarryChanged(this, true);

        if (ShouldUseLegacyFlagVisibility())
            _view.SetFlagVisibility(true);
    }

    public void ResetRoundState()
    {
        enabled = true;
        if (_model == null || _view == null) return;

        _model.IsDead = false;
        _model.MoveInput = 0f;
        _model.TurnInput = 0f;
        _model.CurrentMoveInput = 0f;

        _model.HasFlag = false;
        GameEvents.RaiseFlagCarryChanged(this, false);

        if (ShouldUseLegacyFlagVisibility())
            _view.SetFlagVisibility(false);

        _view.ResetToIdle();

        _fsm?.Transition(PlayerStatesEnum.Idle);
    }

    private void OnDisable()
    {
        GameEvents.OnRoundEnded -= OnRoundEndedHandler;
        if (_model != null)
        {
            _model.CurrentMoveInput = 0f;
        }
    }

    private void OnRoundEndedHandler(bool playerWon)
    {
        if (_model != null)
        {
            _model.MoveInput = 0f;
            _model.TurnInput = 0f;
            _model.CurrentMoveInput = 0f;
        }
        enabled = false;
    }

    bool ShouldUseLegacyFlagVisibility()
    {
        return _flagCarryVisual == null || !_flagCarryVisual.isActiveAndEnabled;
    }

    void InitializeFSM()
    {
        var idle = new PlayerStateIdle<PlayerStatesEnum>(_view, _model, PlayerStatesEnum.Run);
        var run = new PlayerStateRun<PlayerStatesEnum>(_view, _model, transform);
        var dead = new PlayerStateDead<PlayerStatesEnum>(_view, _model);

        _fsm = new FSM<PlayerStatesEnum>(idle);

        idle.AddTransition(PlayerStatesEnum.Run, run);
        idle.AddTransition(PlayerStatesEnum.Dead, dead);

        run.AddTransition(PlayerStatesEnum.Idle, idle);
        run.AddTransition(PlayerStatesEnum.Dead, dead);

        dead.AddTransition(PlayerStatesEnum.Idle, idle);
    }

    void InitializeTree()
    {
        ITreeNode idle = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Idle));
        ITreeNode running = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Run));
        ITreeNode qRun = new QuestionNode(QRun, running, idle);
        _root = qRun;
    }

    bool QRun()
    {
        if (_model == null) return false;
        float threshold = 0.05f;
        bool isCurrentlyRunning = _fsm != null && _fsm.CurrentState is PlayerStateRun<PlayerStatesEnum>;
        if (isCurrentlyRunning)
        {
            return Mathf.Abs(_model.CurrentMoveInput) > threshold;
        }
        else
        {
            return Mathf.Abs(_model.MoveInput) > threshold;
        }
    }

    public void Die()
    {
        if (_model == null || _view == null) return;
        if (_model.IsDead) return;

        _model.IsDead = true;
        _model.MoveInput = 0f;
        _model.TurnInput = 0f;
        _model.CurrentMoveInput = 0f;

        _model.HasFlag = false;
        GameEvents.RaiseFlagCarryChanged(this, false);

        if (ShouldUseLegacyFlagVisibility())
            _view.SetFlagVisibility(false);

        _fsm?.Transition(PlayerStatesEnum.Dead);
    }
}
