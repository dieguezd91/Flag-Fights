using UnityEngine;

public class GoblinStateAttack<T> : State<T>
{
    GoblinController _controller;
    GoblinModel _model;
    GoblinView _view;
    float _lastAttackTime;

    public GoblinStateAttack(GoblinController controller, GoblinModel model, GoblinView view)
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
        }
        else
        {
            _view._animator.SetBool("Idle", false);
            _view._animator.SetBool("Running", true);
        }
    }

    public override void Enter()
    {
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
        UpdateAnimator();
        if (_view._animator != null)
            _view._animator.SetTrigger("Attack");
        _lastAttackTime = Time.time;
    }

    public override void Sleep()
    {
        if (_view._animator != null)
            _view._animator.ResetTrigger("Attack");
    }

    public override void Execute()
    {
        UpdateAnimator();
        Vector3 dirToTarget = _view.LOS.TargetLOS.position - _controller.transform.position;
        dirToTarget.y = 0;
        Vector3 dir = dirToTarget.normalized;
        _view.LookDir(dir);
        if (Time.time - _lastAttackTime < _model.attackCD)
            return;

        if (_view._animator != null)
            _view._animator.SetTrigger("Attack");

        _lastAttackTime = Time.time;
        ResolveAttack();
    }

    private void ResolveAttack()
    {
        Vector3 hitCenter = _controller.transform.position
            + _controller.transform.forward * 0.25f
            + _controller.transform.up * 0.25f;

        Collider[] collidersAhead = Physics.OverlapSphere(hitCenter, 0.2f);

        bool playerHit = false;

        foreach (Collider collider in collidersAhead)
        {
            if (!collider.CompareTag("Player"))
                continue;

            PlayerModel playerModel = collider.GetComponent<PlayerModel>();
            if (playerModel == null || playerModel.IsDead)
                continue;

            playerHit = true;
            break;
        }

        if (playerHit)
        {
            AudioManager.Instance?.PlaySFX(_model.attackSFX);
            GameEvents.RaisePlayerHit();
            return;
        }

        AudioManager.Instance?.PlaySFX(_model.swingSFX);
    }
}
