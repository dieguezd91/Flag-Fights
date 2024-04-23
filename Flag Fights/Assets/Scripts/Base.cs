using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base : MonoBehaviour
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
            for (int n = 0; n < charactersToSpawn; n++)
            {
                var transformSelected = MyRandoms.Roulette(spawnPoints);
                spawnPoints.Remove(transformSelected);
                Instantiate(characterPrefab, transformSelected.position, transformSelected.rotation);
            }
        else Debug.Log("Not enough spawnpoints");
    }
}