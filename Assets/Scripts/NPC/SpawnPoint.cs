using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GlobalGameObjectList GlobalSpawnPointList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GlobalSpawnPointList.Value.Add(gameObject);
    }

    private void OnDestroy()
    {
        GlobalSpawnPointList.Value.Remove(gameObject);
    }
}
