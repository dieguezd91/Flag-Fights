using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    Dictionary<Transform, float> spawnPoints;
    [SerializeField] List<SpawnpointInfo> posibleSpawns;

    [SerializeField] int charactersToSpawn;
    [SerializeField] GameObject characterPrefab;

    public void InitializeBase()
    {
        GetSpawnpoints();
        AsignPositions();
    }

    void GetSpawnpoints()
    {
        spawnPoints = new Dictionary<Transform, float>();
        for (int n = 0; n < posibleSpawns.Count; n++)
        {
            var curr = posibleSpawns[n];
            spawnPoints[curr.transform] = curr.weight;
        }
    }

    void AsignPositions()
    {
        if (charactersToSpawn <= posibleSpawns.Count)
        {
            for (int n = 0; n < charactersToSpawn; n++)
            {
                var transformSelected = MyRandoms.Roulette(spawnPoints);
                spawnPoints.Remove(transformSelected);
                Vector3 spawnPos = transformSelected.position;
                if (Physics.Raycast(transformSelected.position + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
                {
                    spawnPos = hit.point;
                }
                Instantiate(characterPrefab, spawnPos, transformSelected.rotation);
            }
        }
    }
}
