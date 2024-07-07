using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    void LookDir(Vector3 direction);
    void Move(Vector3 direction, float speed);
}
