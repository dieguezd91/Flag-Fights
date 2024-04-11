using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDecisionTree : MonoBehaviour
{
    public int life;
    public bool isIdle;
    public bool hasLOS;
    public bool isOnRange;
    ITreeNode _root;
    private void Awake() => InitializeTree();
    private void Update() => _root.Execute();
    void InitializeTree()
    {
        //Actions
        ITreeNode dead = new ActionNode(() => print("Tree: Dead"));
        ITreeNode idle = new ActionNode(() => StateIdle());
        ITreeNode patrol = new ActionNode(() => StatePatrol());
        ITreeNode chase = new ActionNode(() => StateChase());
        ITreeNode attack = new ActionNode(() => StateAttack());

        //Questions
        ITreeNode qChase = new QuestionNode(QuestionIsOnRange, attack, chase);
        ITreeNode qPatrol = new QuestionNode(QuestionLoS, qChase, patrol);
        ITreeNode qIdle = new QuestionNode(QuestionIdle, idle, qPatrol);
        ITreeNode qHasLife = new QuestionNode(QuestionHasLife, qIdle, dead);

        _root = qHasLife;
    }

    public void ChangeTree(ITreeNode newTree) => _root = newTree;

    public bool QuestionLoS() => hasLOS;
    public bool QuestionHasLife() => life > 0;
    public bool QuestionIsOnRange() => isOnRange;
    public bool QuestionIdle() => isIdle;


    public void StateIdle() { }
    public void StatePatrol() { }
    public void StateChase() { }
    public void StateAttack() { }

}
