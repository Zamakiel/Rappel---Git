using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class WorldManagerScript : MonoBehaviour
{
    public static WorldManagerScript s_instance;

    float m_referenceBuildingFloorHeight = 5;

    [SerializeField]
    Sprite m_buildingRoofSprite;
    [SerializeField]
    public List<Sprite> m_buildingFloorSprites;
    [SerializeField]
    Sprite m_buildingBaseSprite;
    [SerializeField]
    Sprite m_worldFloorSprite;

    [SerializeField]
    GameObject m_buildingRoofGameObject;
    [SerializeField]
    GameObject m_buildingFloorsContainerGameObject;
    [SerializeField]
    public List<GameObject> m_buildingFloorGameObjects;
    [SerializeField]
    GameObject m_buildingBaseGameObject;
    [SerializeField]
    public GameObject m_worldFloorGameObject;
    [SerializeField]
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

        CreateBuilding();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField]
    int m_floors = 2;
    public void CreateBuilding()
    {
        Vector2 floorSize = m_buildingBaseSprite.rect.size;
        floorSize = floorSize / m_buildingBaseSprite.pixelsPerUnit;
        m_buildingBaseGameObject.transform.position = m_worldFloorGameObject.transform.position + new Vector3(0, 0.5f * floorSize.y, 0);

        float previousFloorSize = floorSize.y;
        GameObject previousFloor = m_buildingBaseGameObject;
        for (int floorCounter = 0; floorCounter < m_floors; floorCounter++)
        {
            GameObject floor = Instantiate(Resources.Load("Prefabs/BuildingFloorPrefab")) as GameObject;
            floor.name = "Floor " + floorCounter;

            floorSize = floor.GetComponent<SpriteRenderer>().sprite.rect.size;
            floorSize = floorSize / floor.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            floor.transform.SetParent(m_buildingFloorsContainerGameObject.transform, false);
            floor.transform.position = previousFloor.transform.position + new Vector3(0, 0.5f * (floorSize.y + previousFloorSize), 0);

            m_buildingFloorGameObjects.Add(floor);
            m_buildingFloorSprites.Add(floor.GetComponent<SpriteRenderer>().sprite);

            previousFloorSize = floorSize.y;
            previousFloor = floor;
        }

        floorSize = m_buildingRoofSprite.rect.size;
        floorSize = floorSize / m_buildingRoofSprite.pixelsPerUnit;
        m_buildingRoofGameObject.transform.position = previousFloor.transform.position + new Vector3(0, 0.5f * (floorSize.y + previousFloorSize), 0);

    }
}
