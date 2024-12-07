using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    enum PlayerJumpingStates
    {
        error = -1,
        idle,
        jumping,
        climbing,
        walking,
        count
    }
    [SerializeField]
    PlayerJumpingStates m_jumpingState;

    //Lazy singleton
    public static PlayerScript s_instance;

    [SerializeField]
    bool m_isDownMovementKeyPressed;
    [SerializeField]
    float m_keyDownTime;

    [SerializeField]
    Vector2 m_acceleration;
    [SerializeField]
    Vector2 m_speedPrivate;

    [SerializeField]
    float m_maxYPosition;

    Vector2 m_speed
    {

        get { return m_speedPrivate; }
        set
        {
            m_speedPrivate = value;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        m_isDownMovementKeyPressed = false;
        m_jumpingState = PlayerJumpingStates.idle;
        m_keyDownTime = 0;
        m_acceleration = new Vector2(1.25f, 0);
        m_maxYPosition = 2.5f;

        //InputReaderScript.s_instance.m_downMovementKeyStatusChange += OnDownMovementKeyStatusChange;
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Key state: " + m_keyDownLastState);
        if (m_isDownMovementKeyPressed)
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
        if (m_jumpingState == PlayerJumpingStates.idle && m_isDownMovementKeyPressed != keyStatus)
        {
            m_isDownMovementKeyPressed = keyStatus;
            Debug.Log("Key state changed to: " + m_isDownMovementKeyPressed);
            if (!m_isDownMovementKeyPressed && m_jumpingState == PlayerJumpingStates.idle)
            {
                StartCoroutine(PlayerArchingMoveDownCorutine(m_keyDownTime));
            }
        }
    }

    public void OnUpMovementKeyPress()
    {
        if (m_jumpingState == PlayerJumpingStates.idle)
        {
            StartCoroutine(PlayerMoveUpCorutine());
        }
    }


    public void ResetPlayer()
    {
        transform.localPosition = Vector3.zero;
        m_speed = Vector2.zero;

        m_jumpingState = PlayerJumpingStates.idle;
        m_keyDownTime = 0;
        m_isDownMovementKeyPressed = false;
    }

    IEnumerator PlayerMoveUpCorutine()
    {
        m_jumpingState = PlayerJumpingStates.walking;
        InputReaderScript.s_instance.m_onUpMovementKeyPress -= OnUpMovementKeyPress;
        const float kVerticalTravelTimeSeconds = 3;
        Vector3 direction = new Vector3(-1, -1, 0);
        Vector3 speed = new Vector3(0.25f, transform.localPosition.y / kVerticalTravelTimeSeconds, 0);

        while (transform.localPosition.x != 0)
        {
            float deltaMove = direction.x * speed.x * Time.deltaTime;
            if (math.abs(deltaMove) > math.abs(transform.localPosition.x))
            {
                deltaMove = -transform.localPosition.x;
            }

            transform.localPosition += new Vector3(deltaMove, 0, 0);
            yield return null;
        };

        m_jumpingState = PlayerJumpingStates.climbing;
        while (transform.localPosition.y != 0)
        {
            float deltaMove = direction.y * speed.y * Time.deltaTime;
            if (math.abs(deltaMove) > math.abs(transform.localPosition.y))
            {
                deltaMove = -transform.localPosition.y;
            }

            transform.localPosition += new Vector3(0, deltaMove, 0);
            yield return null;
        }

        InputReaderScript.s_instance.m_onUpMovementKeyPress += OnUpMovementKeyPress;
        m_jumpingState = PlayerJumpingStates.idle;
    }

    IEnumerator PlayerArchingMoveDownCorutine(float keyPressTime)
    {
        m_jumpingState = PlayerJumpingStates.jumping;
        InputReaderScript.s_instance.m_onDownMovementKeyStatusChange -= OnDownMovementKeyStatusChange;
        const float kSqrtRootOfTwo = 1;
        m_speed = new Vector2(kSqrtRootOfTwo, -keyPressTime * keyPressTime);

        do
        {
            m_speed = m_speed - new Vector2(m_acceleration.x * Time.deltaTime, 0);
            var deltaMovement = Time.deltaTime * new Vector3(m_speed.x, m_speed.y, 0);
            if (transform.localPosition.x + deltaMovement.x < 0)
            {
                deltaMovement.x = -transform.localPosition.x;
            }
            if (transform.localPosition.y + deltaMovement.y < -m_maxYPosition)
            {
                deltaMovement.y = -transform.localPosition.y - m_maxYPosition;
            }
            transform.localPosition += deltaMovement;

            Debug.Log("Corutine loop");
            yield return null;
        } while (transform.localPosition.x > 0 && transform.localPosition.y > -m_maxYPosition);

        InputReaderScript.s_instance.m_onDownMovementKeyStatusChange += OnDownMovementKeyStatusChange;

        m_speed = Vector2.zero;
        m_keyDownTime = 0;

        m_jumpingState = PlayerJumpingStates.idle;
    }
}
