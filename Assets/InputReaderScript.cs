using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class InputReaderScript : MonoBehaviour
{
    //Lazy singleton
    private static readonly Lazy<InputReaderScript> lazy =
       new Lazy<InputReaderScript>(() => new InputReaderScript());

    public static InputReaderScript s_instance
    {
        get
        {
            return lazy.Value;
        }
    }

    [SerializeField]
    public KeyCode m_keyUp;
    [SerializeField]
    public KeyCode m_keyDown;

    public delegate void OnDownMovementKeyStatusChangeEvent(bool keyStatus);
    public OnDownMovementKeyStatusChangeEvent m_downMovementKeyStatusChange;

    void Start()
    {

    }

    private void Awake()
    {
        m_keyUp = KeyCode.A;
        m_keyDown = KeyCode.D;

        m_downMovementKeyStatusChange += PlayerScript.s_instance.OnDownMovementKeyStatusChange;

        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    void Update()
    {
        if(m_downMovementKeyStatusChange == null)
        {
            Debug.Log("Event is null");
        }
        if ((Input.GetKeyDown(m_keyDown) || Input.GetKeyUp(m_keyDown)) && m_downMovementKeyStatusChange != null)
        {
            m_downMovementKeyStatusChange(Input.GetKeyDown(m_keyDown));
        }
    }
}
