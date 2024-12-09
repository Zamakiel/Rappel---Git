using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject SpawnableNPCs;
    public List<GameObject> SpawnPoints;

    public float TimeForNextSpawn;
    public Vector2 NextSpawnRandomRange;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeForNextSpawn = Mathf.Max(0, TimeForNextSpawn - Time.deltaTime);
        if(TimeForNextSpawn == 0)
        {
            Instantiate(SpawnableNPCs, SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)].transform);
            TimeForNextSpawn = Random.Range(NextSpawnRandomRange.x, NextSpawnRandomRange.y);
        }
    }
}
