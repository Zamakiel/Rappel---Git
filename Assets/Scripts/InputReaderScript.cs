using UnityEngine;

public class InputReaderScript : MonoBehaviour
{
    private static InputReaderScript s_instancePrivate;
    public static InputReaderScript s_instance { get { return s_instancePrivate; } }    

    [SerializeField]
    public KeyCode m_keyUp;
    [SerializeField]
    public KeyCode m_keyDown;
    [SerializeField]
    public KeyCode m_keyReset;

    public KeyCode m_fireKey;

    public delegate void OnDownMovementKeyStatusChangeEvent(bool keyStatus);
    public OnDownMovementKeyStatusChangeEvent m_onDownMovementKeyStatusChange;

    public delegate void OnUpMovementKeySPressEvent();
    public OnUpMovementKeySPressEvent m_onUpMovementKeyPress;

    public delegate void OnResetKeyPressEvent();
    public OnResetKeyPressEvent m_onResetKeyPress;

    public delegate void OnFireKeyEvent();
    public OnFireKeyEvent m_onFireKeyDown;

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
            s_instancePrivate = this;
        }
        DontDestroyOnLoad(this);

        m_keyUp = KeyCode.W;
        m_keyDown = KeyCode.S;
        m_onDownMovementKeyStatusChange += PlayerScript.s_instance.OnDownMovementKeyStatusChange;
        m_onUpMovementKeyPress += PlayerScript.s_instance.OnUpMovementKeyPress;

        m_keyReset = KeyCode.Space;
        m_onResetKeyPress += PlayerScript.s_instance.ResetPlayer;

        m_fireKey = KeyCode.Q;
        m_onFireKeyDown += ShooterManager.s_instance.Shoot;

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

        if (Input.GetKeyDown(m_keyUp) && m_onResetKeyPress != null)
        {
            m_onUpMovementKeyPress();
        }

        if (Input.GetKey(m_fireKey))
        {
            m_onFireKeyDown();
        }
    }
}
