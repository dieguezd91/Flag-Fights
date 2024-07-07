using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoblinStateIdle<T> : State<T>
{
    GoblinController _controller;
    GoblinView _view;
    GoblinModel _model;

    public GoblinStateIdle(GoblinController controller, GoblinView view, GoblinModel model)
    {
        _controller = controller;
        _view = view;
        _model = model;
    }

    public override void Enter()
    {
        if (_controller == null)
        {
            return;
        }
        if (_view._animator == null)
        {
            return;
        }

        _view._animator.SetBool("Idle", true);
    }
    
    public override void Sleep()
    {
        _view._animator.SetBool("Idle", false);
    }
}
