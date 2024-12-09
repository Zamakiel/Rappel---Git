using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class WorldManagerScript : MonoBehaviour
{
    public static WorldManagerScript s_instance;

    List<Sprite> m_entireBuildingOrderedSprites;
    public List<GameObject> m_entireBuildingOrderedGameObjects;

    public GameObject m_buildingRoofGameObject;
    public List<GameObject> m_buildingFloorGameObjects;
    public GameObject m_buildingBaseGameObject;
    public GameObject m_worldFloorGameObject;
    public GameObject m_craneAnchorPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(this);
        }
        else
        {
            s_instance = this;
        }

        if (!m_initialized)
        {
            Initialize();
        }
    }

    public bool m_initialized = false;
    public void Initialize()
    {
        CreateBuilding();

        m_initialized = true;
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    public enum WindowTypes
    {
        error = -1,
        left,
        center,
        right,
        count

    }

    public Transform GetWindowPosition(int floor, WindowTypes windowType)
    {
        if (floor > m_entireBuildingOrderedGameObjects.Count)
        {
            Debug.LogWarning("Tried to get window of non existing floor");
            return transform;
        }

        GameObject currentFloor = m_entireBuildingOrderedGameObjects[floor];
        if (currentFloor.transform.childCount <= (int)windowType)
        {
            Debug.LogWarning("Tried to get non existing window of " + currentFloor.name);
            return transform;
        }

        return currentFloor.transform.GetChild((int)windowType);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField]
    int m_floors = 2;
    public void CreateBuilding()
    {
        float previousFloorSize = 0;
        GameObject previousFloor = null;
        int m_totalFloors = m_floors + 2;
        m_entireBuildingOrderedSprites = new List<Sprite>();
        for (int floorCounter = 0; floorCounter < m_totalFloors; floorCounter++)
        {
            GameObject floor;
            if (floorCounter == 0)
            {
                floor = Instantiate(Resources.Load("Prefabs/BuildingBasePrefab")) as GameObject;
                floor.name = "Building Base";
                m_buildingBaseGameObject = floor;
            }
            else if (floorCounter == m_totalFloors - 1)
            {
                floor = Instantiate(Resources.Load("Prefabs/BuildingRoofPrefab")) as GameObject;
                floor.name = "Building Roof";
                m_buildingRoofGameObject = floor;
                m_craneAnchorPoint = m_buildingRoofGameObject.transform.GetChild(0).GetChild(0).gameObject;
            }
            else
            {
                floor = Instantiate(Resources.Load("Prefabs/BuildingFloorPrefab")) as GameObject;
                floor.name = "Building Floor " + floorCounter;
                m_buildingFloorGameObjects.Add(floor);
            }

            var floorSize = floor.GetComponent<SpriteRenderer>().sprite.rect.size;
            floorSize = floorSize / floor.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            floor.transform.SetParent(transform, false);
            floor.transform.position = new Vector3(0, 0.5f * (floorSize.y + previousFloorSize), 0);
            if (previousFloor != null)
            {
                floor.transform.position += previousFloor.transform.position;
            }

            m_entireBuildingOrderedGameObjects.Add(floor);
            m_entireBuildingOrderedSprites.Add(floor.GetComponent<SpriteRenderer>().sprite);

            previousFloorSize = floorSize.y;
            previousFloor = floor;
        }
    }
}
