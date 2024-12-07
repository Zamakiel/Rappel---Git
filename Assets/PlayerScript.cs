using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Lazy singleton
    private static readonly Lazy<PlayerScript> lazy =
       new Lazy<PlayerScript>(() => new PlayerScript());

    public static PlayerScript s_instance
    {
        get
        {
            return lazy.Value;
        }
    }

    bool m_keyDownLastState;
    bool m_isJumping;
    float m_keyDownTime;



    Vector2 m_position;
    Vector2 m_maxSpeed;
    Vector2 m_acceleration;
    Vector2 m_speedPrivate;
    Vector2 m_speed
    {

        get { return m_speedPrivate; }
        set
        {
            m_speedPrivate = value;
            if (m_speedPrivate.x < 0)
            {
                m_speedPrivate.x = 0;
            }
            if (m_speedPrivate.y < 0)
            {
                m_speedPrivate.y = 0;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        m_keyDownLastState = true;
        m_isJumping = false;
        m_keyDownTime = 0;

        //InputReaderScript.s_instance.m_downMovementKeyStatusChange += OnDownMovementKeyStatusChange;
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Key state: " + m_keyDownLastState);
        if (m_keyDownLastState)
        {
            m_keyDownTime += Time.deltaTime;
            Debug.Log("Charging jump, current charge: " + (m_keyDownTime * m_keyDownTime));
        }
        else
        {
            //Debug.Log("Not charging jump");
        }
    }

    public void OnDownMovementKeyStatusChange(bool keyStatus)
    {
        if (m_keyDownLastState != keyStatus)
        {
            m_keyDownLastState = keyStatus;
            Debug.Log("Key state changed to: " + m_keyDownLastState);
            if (!m_keyDownLastState && !m_isJumping)
            {
                //StartCoroutine(PlayerArchMoveCorutine(m_keyDownTime));
                //InputReaderScript.s_instance.m_downMovementKeyStatusChange -= OnDownMovementKeyStatusChange;
            }
        }
    }

    IEnumerator PlayerArchMoveCorutine(float keyPressTime)
    {
        m_isJumping = true;
        float kLaunchSpeed = keyPressTime * keyPressTime;
        const float kSqrtRootOfTwo = 1.14f;
        m_speed = kSqrtRootOfTwo * new Vector2(kLaunchSpeed, kLaunchSpeed);

        while (m_speed.x > 0)
        {
            m_speed = m_speed - new Vector2(m_acceleration.x * Time.deltaTime, 0);
            transform.localPosition += Time.deltaTime * new Vector3(m_speed.x, m_speed.y, 0);
            yield return null;
        }

        InputReaderScript.s_instance.m_downMovementKeyStatusChange += OnDownMovementKeyStatusChange;
        m_isJumping = false;
    }
}
