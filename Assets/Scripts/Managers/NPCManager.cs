using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject SpawnableNPCs;
    public GlobalGameObjectList SpawnPoints;

    public float TimeForNextSpawn;
    public Vector2 NextSpawnRandomRange;

    List<int> SpawnIndexes;



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
            if(SpawnIndexes == null)
            {
                SpawnIndexes = new List<int>();
                for (int i = 0; i < SpawnPoints.Value.Count; i++)
                {
                    SpawnIndexes.Add(i);
                }
            }
            if (SpawnIndexes.Count > 0)
            {
                int randomIndex = SpawnIndexes[Random.Range(0, SpawnIndexes.Count - 1)];
                SpawnIndexes.Remove(randomIndex);
                NPCController controller = Instantiate(SpawnableNPCs, SpawnPoints.Value[randomIndex].transform).GetComponent<NPCController>();
                controller.SpawnedIndex = randomIndex;
                TimeForNextSpawn = Random.Range(NextSpawnRandomRange.x, NextSpawnRandomRange.y);
            }            
        }
    }

    public void RefreshIndex(NPCController controller)
    {
        SpawnIndexes.Add(controller.SpawnedIndex);
    }

}
