using Mono.Cecil.Cil;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
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
            s_instance = this;
        }

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

        if (Input.GetKeyDown(m_keyUp))
        {
            m_onUpMovementKeyPress();
        }

        if (Input.GetKey(m_fireKey))
        {
            m_onFireKeyDown();
        }
    }
}
