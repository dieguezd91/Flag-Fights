using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{

    Dictionary<Transform, float> spawnpoints;
    [SerializeField] List<SpawnpointInfo> posibleSpawns;
    [SerializeField] GameObject flagPrefab;

    int pointsDiff;

    public void InitializeSpawner()
    {
        pointsDiff = GameManager.instance.Points - GameManager.instance.EnemyPoints;
        GetSpawnpoints();
        GameObject newFlag = SpawnFlag();
        GameManager.instance.Flag = newFlag;
    }

    void SetWeight(SpawnpointInfo spawnpoint)
    {
        float multiplier = 1;
        if (pointsDiff > 0)
        {
            if (spawnpoint.closeToPlayerSpawn) multiplier = 2;
            else multiplier = .5f;
        }
        else if (pointsDiff < 0)
        {
            if (spawnpoint.closeToPlayerSpawn) multiplier = .5f;
            else multiplier = 2;
        }
        spawnpoint.weight *= multiplier;
    }

    void GetSpawnpoints()
    {
        spawnpoints = new Dictionary<Transform, float>();
        for (int n = 0; n < posibleSpawns.Count; n++)
        {
            var curr = posibleSpawns[n];
            SetWeight(curr);
            spawnpoints[curr.transform] = curr.weight;
        }
    }

    GameObject SpawnFlag()
    {
        var spawnSelected = MyRandoms.Roulette(spawnpoints);
        return Instantiate(flagPrefab, spawnSelected.position, spawnSelected.rotation);
    }
}
