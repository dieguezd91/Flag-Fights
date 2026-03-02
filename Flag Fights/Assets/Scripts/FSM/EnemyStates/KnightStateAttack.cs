using UnityEngine;

public class KnightStateAttack<T> : State<T>
{
    KnightController _controller;
    KnightView _view;
    KnightModel _model;
    float _lastAttackTime;

    public KnightStateAttack(KnightController controller, KnightModel model, KnightView view)
    {
        _controller = controller;
        _model = model;
        _view = view;
    }

    public override void Enter()
    {
        _view.RB.velocity = new Vector3(0, _view.RB.velocity.y, 0);
        _view.PlayAttackAnimation();
        _lastAttackTime = Time.time;
    }

    public override void Sleep()
    {
        _view._animator.ResetTrigger("Attack");
    }

    public override void Execute()
    {
        base.Execute();
        TryAttack();
    }

    void TryAttack()
    {
        if (Time.time - _lastAttackTime < _model.attackCD) return;

        _view.PlayAttackAnimation();
        _lastAttackTime = Time.time;
        Collider[] collidersAhead = Physics.OverlapSphere(
            _view.transform.position + _view.transform.forward * 0.35f + _view.transform.up * 0.5f, 0.4f);
        foreach (Collider col in collidersAhead)
        {
            if (col.CompareTag("Player"))
            {
                AudioManager.Instance?.PlaySFX(_model.attackSFX);
                GameEvents.OnPlayerHit?.Invoke();
            }
            else AudioManager.Instance?.PlaySFX(_model.swingSFX);
        }
    }
}
