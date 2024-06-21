using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Flag management
    public bool hasFlag = false;
    public GameObject flag;

    public Animator Animator => _animator;
    Animator _animator;

    //Stats
    public float Speed => _speed;
    [SerializeField] float _speed;
    public float TurnSpeed => _turnSpeed;
    [SerializeField] float _turnSpeed;

    //FSM
    FSM<PlayerStatesEnum> _fsm;
    ITreeNode _root;

    public Vector2 MovementInput => _movementInput;
    Vector2 _movementInput = new Vector2(0, 0);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        InitializeFSM();
        InitializeTree();
    }

    void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
    }

    //Check collision with flag
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Flag"))
        {
            collision.gameObject.SetActive(false);
            flag.SetActive(true);
            hasFlag = true;
        }
    }

    void InitializeFSM()
    {
        //States declarations
        var idle = new PlayerStateIdle<PlayerStatesEnum>(_animator, _speed, PlayerStatesEnum.Run);
        var run = new PlayerStateRun<PlayerStatesEnum>(this);

        //Create Finite State Machine
        _fsm = new FSM<PlayerStatesEnum>(idle);

        //Create transitions between states
        idle.AddTransition(PlayerStatesEnum.Run, run);
        run.AddTransition(PlayerStatesEnum.Idle, idle);
    }

    void InitializeTree()
    {
        //Actions
        ITreeNode idle = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Idle));
        ITreeNode running = new ActionNode(() => _fsm.Transition(PlayerStatesEnum.Run));

        //Questions
        ITreeNode qRun = new QuestionNode(QRun, running, idle);

        //First node to execute
        _root = qRun;
    }

    bool QRun() => _movementInput != Vector2.zero;
}
