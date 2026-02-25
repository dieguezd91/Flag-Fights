using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float multiplier;
    Transform _player;
    public bool isActive;

    private void Awake()
    {
        _player = GameManager.instance.player.transform;
    }

    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        if (!isActive) return Vector3.zero;

        GoblinController leader = GoblinController.CurrentLeader;
        if (leader == null) return Vector3.zero;

        Transform target;
        var selfGoblin = self as GoblinController;
        if (selfGoblin != null && selfGoblin == leader)
            target = _player;
        else
            target = leader.transform;

        Vector3 dir = (target.position - self.Position).normalized * multiplier;
        Debug.DrawRay(self.Position, dir);
        return dir;
    }
}
