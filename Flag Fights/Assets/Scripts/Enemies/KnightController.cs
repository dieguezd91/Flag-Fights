using UnityEngine;

public class KnightController : EnemyController
{
    [HideInInspector] public KnightModel Model;
    [HideInInspector] public KnightView View;

    FSM<KnightStatesEnum> _fsm;
    KnightStatePatrol<KnightStatesEnum> _patrolState;
    NodeRegistry _nodeRegistry;
    public NodeRegistry NodeRegistry => _nodeRegistry;

    Vector3 _initialPosition;
    Quaternion _initialRotation;

    public override void Awake()
    {
        Model = GetComponent<KnightModel>();
        View = GetComponent<KnightView>();
        Model.lastTargetPosKnown = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        InitializeFSM();
        InitializeTree();
    }

    public override void Start()
    {
        if (GameManager.instance?.player != null)
            View.LineOfSight.SetTarget(GameManager.instance.player.transform);
        _nodeRegistry = FindObjectOfType<NodeRegistry>();
    }

    KnightStateDefeat<KnightStatesEnum> _roundDefeatState;

    public void EnterRoundDefeat()
    {
        if (_fsm == null || _roundDefeatState == null) return;
        if (_fsm.CurrentState == _roundDefeatState) return;

        _fsm.Transition(KnightStatesEnum.RoundDefeat);
    }

    public void ResetForRound()
    {
        if (View != null && View._animator != null)
        {
            View._animator.ResetTrigger("Defeat");
        }
        ResetEnemy();
    }

    public void ResetEnemy()
    {
        View.RB.velocity = Vector3.zero;
        View.RB.angularVelocity = Vector3.zero;

        transform.SetPositionAndRotation(_initialPosition, _initialRotation);

        Model.lastTargetPosKnown = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Model.isFinishPath = true;
        Model.isIdle = true;

        if (View._animator != null)
        {
            View._animator.SetBool("Running", false);
            View._animator.SetBool("Patrolling", false);
        }

        _fsm.Transition(KnightStatesEnum.Idle);
    }

    public override void Update()
    {
        _fsm.OnUpdate();
        if (_fsm.CurrentState != _roundDefeatState)
        {
            _root.Execute();
        }
    }

    public override void InitializeFSM()
    {
        _patrolState = new KnightStatePatrol<KnightStatesEnum>(this, Model, View);
        var chase  = new KnightStateChase<KnightStatesEnum>(this, Model, View);
        var attack = new KnightStateAttack<KnightStatesEnum>(this, Model, View);
        var idle   = new KnightStateIdle<KnightStatesEnum>(this, Model, View);
        _roundDefeatState = new KnightStateDefeat<KnightStatesEnum>(this, Model, View);

        _fsm = new FSM<KnightStatesEnum>(idle);

        _patrolState.AddTransition(KnightStatesEnum.Chase,  chase);
        _patrolState.AddTransition(KnightStatesEnum.Attack, attack);
        _patrolState.AddTransition(KnightStatesEnum.Idle,   idle);
        _patrolState.AddTransition(KnightStatesEnum.RoundDefeat, _roundDefeatState);

        chase.AddTransition(KnightStatesEnum.Patrol, _patrolState);
        chase.AddTransition(KnightStatesEnum.Idle,   idle);
        chase.AddTransition(KnightStatesEnum.Attack, attack);
        chase.AddTransition(KnightStatesEnum.RoundDefeat, _roundDefeatState);

        attack.AddTransition(KnightStatesEnum.Chase,  chase);
        attack.AddTransition(KnightStatesEnum.Patrol, _patrolState);
        attack.AddTransition(KnightStatesEnum.Idle,   idle);
        attack.AddTransition(KnightStatesEnum.RoundDefeat, _roundDefeatState);

        idle.AddTransition(KnightStatesEnum.Chase,  chase);
        idle.AddTransition(KnightStatesEnum.Patrol, _patrolState);
        idle.AddTransition(KnightStatesEnum.Attack, attack);
        idle.AddTransition(KnightStatesEnum.RoundDefeat, _roundDefeatState);

        _roundDefeatState.AddTransition(KnightStatesEnum.Idle, idle);
    }

    public override void InitializeTree()
    {
        ITreeNode patrol = new ActionNode(() => _fsm.Transition(KnightStatesEnum.Patrol));
        ITreeNode chase  = new ActionNode(() => _fsm.Transition(KnightStatesEnum.Chase));
        ITreeNode attack = new ActionNode(() => _fsm.Transition(KnightStatesEnum.Attack));
        ITreeNode idle   = new ActionNode(() => _fsm.Transition(KnightStatesEnum.Idle));

        ITreeNode qPatrol = new QuestionNode(QPatrol, patrol, idle);
        ITreeNode qChase  = new QuestionNode(QChase,  chase,  qPatrol);
        ITreeNode qAttack = new QuestionNode(QAttack, attack, qChase);

        _root = qAttack;
    }

    public bool QAttack() => View.LineOfSight.HasLOS(Model.attackRange);

    public bool QChase()
    {
        if (View.LineOfSight.HasLOS(View.LineOfSight.Vision) ||
            View.LineOfSight.HasLOS(View.LineOfSight.Vision, Model.lastTargetPosKnown))
        {
            if (Time.time >= Model.lastCheck + Model.checkCooldown)
            {
                Model.lastTargetPosKnown = View.LineOfSight.TargetLOS.position;
                Model.lastCheck = Time.time;
            }
            return true;
        }
        return false;
    }

    public bool QPatrol() => !Model.isFinishPath;

    public IPoints GetStateWaypoints => _patrolState;
}
