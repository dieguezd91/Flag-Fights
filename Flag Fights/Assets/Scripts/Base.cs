using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    List<Transform> spawnPositions;
    [SerializeField] int charactersToSpawn;
    [SerializeField] GameObject characterPrefab;

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
        if (charactersToSpawn <= spawnPositions.Count)
        {
            for (int i = 0; i < charactersToSpawn; i++)
            {
                int r = Random.Range(0, spawnPositions.Count);
                Instantiate(characterPrefab, spawnPositions[r].position, Quaternion.identity);
                spawnPositions.Remove(spawnPositions[r]);
            }
        }
        else Debug.Log("Not enough positions");
    }
}