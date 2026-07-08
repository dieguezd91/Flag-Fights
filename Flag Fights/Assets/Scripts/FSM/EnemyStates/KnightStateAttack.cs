using UnityEngine;

public class KnightStateAttack<T> : State<T>
{
    KnightController _controller;
    KnightView _view;
    KnightModel _model;

    private bool _isAttacking;
    private bool _hasAppliedDamage;
    private float _attackStartTime;
    private float _nextAttackAllowedTime;

    public KnightStateAttack(KnightController controller, KnightModel model, KnightView view)
    {
        _controller = controller;
        _model = model;
        _view = view;
    }

    private void UpdateAnimator()
    {
        if (_view._animator == null) return;
        float speed = new Vector3(_view.RB.velocity.x, 0, _view.RB.velocity.z).magnitude;
        if (speed <= 0.1f)
        {
            _view._animator.SetBool("Idle", true);
            _view._animator.SetBool("Running", false);
            _view._animator.SetBool("Patrolling", false);
        }
        else
        {
            _view._animator.SetBool("Idle", false);
            _view._animator.SetBool("Running", true);
            _view._animator.SetBool("Patrolling", false);
        }
    }

    public override void Enter()
    {
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
        UpdateAnimator();
        FaceTarget();
        
        if (Time.time >= _nextAttackAllowedTime)
        {
            TryStartAttack();
        }
    }

    public override void Sleep()
    {
        if (_view._animator != null)
            _view._animator.ResetTrigger("Attack");
        _isAttacking = false;
    }

    public override void Execute()
    {
        base.Execute();
        UpdateAnimator();

        if (_isAttacking)
        {
            _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
            UpdateAttack();
        }
        else
        {
            if (_view.LineOfSight.TargetLOS != null && Time.time >= _nextAttackAllowedTime)
            {
                TryStartAttack();
            }
        }
    }

    private void TryStartAttack()
    {
        if (_isAttacking) return;
        if (Time.time < _nextAttackAllowedTime) return;
        if (_view.LineOfSight.TargetLOS == null) return;

        FaceTarget();

        _isAttacking = true;
        _hasAppliedDamage = false;
        _attackStartTime = Time.time;

        _view.PlayAttackAnimation();

        if (_model.debugAttack)
        {
            float dist = Vector3.Distance(_view.transform.position, _view.LineOfSight.TargetLOS.position);
            Debug.Log($"[Attack] Started at {Time.time}. Target distance: {dist}");
        }
    }

    private void UpdateAttack()
    {
        // Face the target before impact
        if (!_hasAppliedDamage)
        {
            FaceTarget();
        }

        if (!_hasAppliedDamage && Time.time >= _attackStartTime + _model.attackImpactDelay)
        {
            FaceTarget(); // Face target immediately before hit check
            ApplyAttackHit();
        }

        if (Time.time >= _attackStartTime + _model.attackRecoveryTime)
        {
            _isAttacking = false;
            _nextAttackAllowedTime = Time.time + _model.attackCD;
        }
    }

    private void ApplyAttackHit()
    {
        Vector3 hitCenter = GetHitCenter();
        Collider[] colliders = _model.hitLayerMask.value != 0
            ? Physics.OverlapSphere(hitCenter, _model.hitRadius, _model.hitLayerMask)
            : Physics.OverlapSphere(hitCenter, _model.hitRadius);

        bool playerHit = false;
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                playerHit = true;
                break;
            }
        }

        if (playerHit)
        {
            AudioManager.Instance?.PlaySFX(_model.attackSFX);
            GameEvents.RaisePlayerHit();
            if (_model.debugAttack)
            {
                float dist = _view.LineOfSight.TargetLOS != null ? Vector3.Distance(_view.transform.position, _view.LineOfSight.TargetLOS.position) : -1f;
                Debug.Log($"[Attack] Hit check executed at {Time.time}. Target distance: {dist}. Hit Player!");
            }
        }
        else
        {
            AudioManager.Instance?.PlaySFX(_model.swingSFX);
            if (_model.debugAttack)
            {
                float dist = _view.LineOfSight.TargetLOS != null ? Vector3.Distance(_view.transform.position, _view.LineOfSight.TargetLOS.position) : -1f;
                string reason = _view.LineOfSight.TargetLOS == null ? "Target is null" : "Target out of hit volume/not detected";
                Debug.Log($"[Attack] Hit check executed at {Time.time}. Target distance: {dist}. Missed. Reason: {reason}");
            }
        }

        _hasAppliedDamage = true;
    }

    private void FaceTarget()
    {
        Transform target = _view.LineOfSight.TargetLOS;
        if (target == null) return;

        Vector3 dir = target.position - _view.transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
        {
            _view.LookDir(dir.normalized);
        }
    }

    private Vector3 GetHitCenter()
    {
        return _view.transform.position
            + _view.transform.forward * _model.hitForwardOffset
            + Vector3.up * _model.hitVerticalOffset;
    }
}
