using UnityEngine;

public class KnightStateDefeat<T> : State<T>
{
    private static readonly int DefeatTrigger = Animator.StringToHash("Defeat");

    private KnightController _controller;
    private KnightModel _model;
    private KnightView _view;

    public KnightStateDefeat(KnightController controller, KnightModel model, KnightView view)
    {
        _controller = controller;
        _model = model;
        _view = view;
    }

    public override void Enter()
    {
        if (_view == null) return;

        // Clear other animation states
        if (_view._animator != null)
        {
            _view._animator.SetBool("Running", false);
            _view._animator.SetBool("Patrolling", false);
            _view._animator.SetBool("Idle", false);
            _view._animator.SetTrigger(DefeatTrigger);
        }

        // Freeze Rigidbody
        if (_view.RB != null)
        {
            _view.RB.velocity = Vector3.zero;
            _view.RB.angularVelocity = Vector3.zero;
        }
    }

    public override void Execute()
    {
        // Maintain freeze during the defeat sequence
        if (_view != null && _view.RB != null)
        {
            _view.RB.velocity = Vector3.zero;
            _view.RB.angularVelocity = Vector3.zero;
        }
    }
}
