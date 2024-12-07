using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateManagerScript : MonoBehaviour
{
    //Lazy singleton
    private static readonly Lazy<StateManagerScript> lazy =
       new Lazy<StateManagerScript>(() => new StateManagerScript());

    public static StateManagerScript s_instance
    {
        get
        {
            return lazy.Value;
        }
    }

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
        m_playerManager.AddComponent<PlayerScript>();
        m_inputManager.AddComponent<InputReaderScript>();
        
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
