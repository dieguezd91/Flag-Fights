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

    public override void Enter()
    {
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
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
        Vector3 dirToTarget = _view.LOS.TargetLOS.position - _controller.transform.position;
        dirToTarget.y = 0;
        Vector3 dir = dirToTarget.normalized;
        _view.LookDir(dir);
        if (Time.time - _lastAttackTime >= _model.attackCD)
        {
            if (_view._animator != null)
                _view._animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
            Collider[] collidersAhead = Physics.OverlapSphere(_controller.transform.position + _controller.transform.forward * .25f + _controller.transform.up * .25f, .2f);
            foreach (Collider col in collidersAhead)
            {
                if (col.CompareTag("Player"))
                {
                    AudioManager.Instance?.PlaySFX(_model.attackSFX);
                    GameEvents.RaisePlayerHit();
                }
                else AudioManager.Instance?.PlaySFX(_model.swingSFX);
            }
        }
    }
}
