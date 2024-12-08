using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public static EnemyManagerScript s_instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    [SerializeField]
    Vector2 m_enemySpawnTimer;
    float m_timeSinceLastSpawn;
    [SerializeField]
    float m_enemySpawnChance;
    // Update is called once per frame
    void Update()
    {
        if (m_enableEnemySpawn)
        {
            bool shouldSpawn = false;
            m_timeSinceLastSpawn += Time.deltaTime;

            if (m_timeSinceLastSpawn > m_enemySpawnTimer.x)
            {
                if (m_timeSinceLastSpawn > m_enemySpawnTimer.y)
                {
                    shouldSpawn = true;
                }
                else
                {
                    float spawnChanceRoll = Random.value;
                    if (spawnChanceRoll > m_enemySpawnChance)
                    {
                        shouldSpawn = true;
                    }
                }
            }

            if (shouldSpawn)
            {
                var windowToSpawn = (WorldManagerScript.WindowTypes)Random.Range(0, 3);
                var positionToSpawn = WorldManagerScript.s_instance.GetWindowPosition(0, windowToSpawn);
                SpawnEnemy(positionToSpawn);
                m_timeSinceLastSpawn = 0;
            }
        }
    }

    void SpawnEnemy(Transform spawnPosition)
    {
        GameObject enemy = Instantiate(Resources.Load("Prefabs/EnemySkull")) as GameObject;
        enemy.transform.parent = transform;
        enemy.transform.position = spawnPosition.position;
    }

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(this);
        }
        else
        {
            s_instance = this;
        }
        DontDestroyOnLoad(this);

        if (!m_initialized) { Initialize(); }
    }

    public bool m_initialized = false;
    public void Initialize()
    {
        m_enableEnemySpawn = false;
        m_enemySpawnChance = 0.1f;
        m_enemySpawnTimer = new Vector2(3, 5);
        Random.InitState((int)System.DateTime.Now.Ticks);

        m_initialized = true;
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }


    [SerializeField]
    bool m_enableEnemySpawn;
    public void EnableDisableEnemySpawn()
    {
        m_enableEnemySpawn = !m_enableEnemySpawn;
    }
}
