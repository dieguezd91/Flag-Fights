using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ITreeNode _root;

    public ISteering _steering;

    public virtual void Awake() { }

    public virtual void Start() { }

    public virtual void Update() { }

    public virtual void InitializeSteerings() { }

    public virtual void InitializeTree() { }

    public virtual void InitializeFSM() { }

}
