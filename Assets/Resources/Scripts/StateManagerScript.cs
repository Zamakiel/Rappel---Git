using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateManagerScript : MonoBehaviour
{
    public static StateManagerScript s_instance;

    [SerializeField]
    GameObject m_playerManager;
    [SerializeField]
    GameObject m_inputManager;

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

        m_playerManager.AddComponent<PlayerScript>();
        m_inputManager.AddComponent<InputReaderScript>();

        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
