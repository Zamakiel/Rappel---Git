using UnityEngine;

public class InputReaderScript : MonoBehaviour
{
    public static InputReaderScript s_instance;

    [SerializeField]
    public KeyCode m_keyUp;
    [SerializeField]
    public KeyCode m_keyDown;
    [SerializeField]
    public KeyCode m_keyReset;
    [SerializeField]
    public KeyCode m_fireKey;
    [SerializeField]
    public KeyCode m_enemyStartKey;

    public delegate void OnDownMovementKeyStatusChangeEvent(bool keyStatus);
    public OnDownMovementKeyStatusChangeEvent m_onDownMovementKeyStatusChange;

    public delegate void OnUpMovementKeyPressEvent();
    public OnUpMovementKeyPressEvent m_onUpMovementKeyPress;

    public delegate void OnResetKeyPressEvent();
    public OnResetKeyPressEvent m_onResetKeyPress;

    public delegate void OnFireKeyEvent();
    public OnFireKeyEvent m_onFireKeyDown;

    public delegate void OnEnemyStartKeyPressEvent();
    public OnFireKeyEvent m_onEnemyStartKeyPress;

    void Start()
    {

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
        m_keyUp = KeyCode.W;
        m_keyDown = KeyCode.S;
        m_onDownMovementKeyStatusChange += PlayerScript.s_instance.OnDownMovementKeyStatusChange;
        m_onUpMovementKeyPress += PlayerScript.s_instance.OnUpMovementKeyPress;

        m_keyReset = KeyCode.Space;
        m_onResetKeyPress += PlayerScript.s_instance.ResetPlayer;

        m_fireKey = KeyCode.Q;
        m_onFireKeyDown += ShooterManager.s_instance.Shoot;

        m_enemyStartKey = KeyCode.E;
        m_onEnemyStartKeyPress += EnemyManagerScript.s_instance.EnableDisableEnemySpawn;

        m_initialized = true;
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    void Update()
    {
        if ((Input.GetKeyDown(m_keyDown) || Input.GetKeyUp(m_keyDown)) && m_onDownMovementKeyStatusChange != null)
        {
            m_onDownMovementKeyStatusChange(Input.GetKeyDown(m_keyDown));
        }

        if (Input.GetKeyDown(m_keyReset))
        {
            m_onResetKeyPress();
        }

        if (Input.GetKeyDown(m_keyUp) && m_onUpMovementKeyPress != null)
        {
            m_onUpMovementKeyPress();
        }

        if (Input.GetKey(m_fireKey))
        {
            m_onFireKeyDown();
        }

        if (Input.GetKeyDown(m_enemyStartKey))
        {
            m_onEnemyStartKeyPress();
        }
    }
}
