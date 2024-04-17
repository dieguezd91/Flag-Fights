using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    List<Transform> spawnPositions;
    [SerializeField] int enemiesToSpawn;
    [SerializeField] GameObject enemyPrefab;

    private void Start()
    {
        GetSpawnpoints();
        AsignPositions();
    }

    void GetSpawnpoints()
    {
        spawnPositions = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>())
            if (t != transform)
                spawnPositions.Add(t);

    }

    void AsignPositions()
    {
        if (enemiesToSpawn <= spawnPositions.Count)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                int r = Random.Range(0, spawnPositions.Count);
                Instantiate(enemyPrefab, spawnPositions[r].position, Quaternion.identity);
                spawnPositions.Remove(spawnPositions[r]);
            }
        }
        else Debug.Log("Not enough positions");
    }
}