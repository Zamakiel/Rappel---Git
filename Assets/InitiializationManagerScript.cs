using UnityEngine;
using UnityEngine.InputSystem;

public class InitiializationManagerScript : MonoBehaviour
{
    public static InitiializationManagerScript s_instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField]
    GameObject m_worldManagerGameObject;
    [SerializeField]
    GameObject m_actorManagerGameObject;
    [SerializeField]
    GameObject m_inputManagerGameObject;
    [SerializeField]
    GameObject m_shooterManagerGameObject;

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

        m_worldManagerGameObject = Instantiate(Resources.Load("Prefabs/WorldManagerPrefab")) as GameObject;
        if (!WorldManagerScript.s_instance.m_initialized)
        {
            WorldManagerScript.s_instance.Initialize();
        }

        m_actorManagerGameObject = Instantiate(Resources.Load("Prefabs/ActorManagerPrefab")) as GameObject;
        if (!PlayerScript.s_instance.m_initialized)
        {
            PlayerScript.s_instance.Initialize();
        }
        if (!EnemyManagerScript.s_instance.m_initialized)
        {
            EnemyManagerScript.s_instance.Initialize();
        }

        m_shooterManagerGameObject = Instantiate(Resources.Load("Prefabs/ShooterManagerPrefab")) as GameObject;
        if (!ShooterManager.s_instance.m_initialized)
        {
            ShooterManager.s_instance.Initialize();
        }

        m_inputManagerGameObject = Instantiate(Resources.Load("Prefabs/InputManagerPrefab")) as GameObject;
        if (!InputReaderScript.s_instance.m_initialized)
        {
            InputReaderScript.s_instance.Initialize();
        }
    }

}
