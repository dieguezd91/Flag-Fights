using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{

    Dictionary<Transform, float> spawnpoints;
    [SerializeField] List<SpawnpointInfo> posibleSpawns;
    [SerializeField] GameObject flagPrefab;

    int pointsDiff;
    Transform _lastSpawnPoint;

    public void InitializeSpawner()
    {
        if (GameManager.instance.Flag != null)
            Destroy(GameManager.instance.Flag);

        pointsDiff = GameManager.instance.Points - GameManager.instance.EnemyPoints;
        GetSpawnpoints();
        GameObject newFlag = SpawnFlag();
        GameManager.instance.Flag = newFlag;
    }

    float GetWeight(SpawnpointInfo spawnpoint)
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
        return spawnpoint.weight * multiplier;
    }

    void GetSpawnpoints()
    {
        spawnpoints = new Dictionary<Transform, float>();
        bool hasAlternatives = posibleSpawns.Count > 1;
        for (int n = 0; n < posibleSpawns.Count; n++)
        {
            var curr = posibleSpawns[n];
            if (hasAlternatives && curr.transform == _lastSpawnPoint) continue;
            spawnpoints[curr.transform] = GetWeight(curr);
        }
    }

    GameObject SpawnFlag()
    {
        var spawnSelected = MyRandoms.Roulette(spawnpoints);
        _lastSpawnPoint = spawnSelected;
        Vector3 spawnPos = spawnSelected.position;

        if (Physics.Raycast(spawnSelected.position + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
            spawnPos = hit.point;

        return Instantiate(flagPrefab, spawnPos, spawnSelected.rotation);
    }
}
