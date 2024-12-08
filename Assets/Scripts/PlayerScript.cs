using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum PlayerJumpingStates
    {
        error = -1,
        idle,
        jumping,
        climbing,
        walking,
        count
    }


    public PlayerJumpingStates m_jumpingState;

    //Lazy singleton
    public static PlayerScript s_instance;

    [SerializeField]
    bool m_isDownMovementKeyPressed;
    [SerializeField]
    float m_keyDownTime;

    [SerializeField]
    float m_maxYPosition;

    [SerializeField]
    GameEvent m_playerJumpingEvent;

    [SerializeField]
    GameEvent m_playerIdleEvent;

    [SerializeField]
    GameEvent m_playerWalkingEvent;

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
        m_maxYPosition = 2.5f;

        m_maxHorizontalJumpDistance = 10;
        m_maxVerticalJumpDistance = 10;
        m_maxJumpHoldChargeTime = 10;
        m_jumpFlyTime = 1;
        m_anchorPosition = Vector3.zero;

        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Key state: " + m_keyDownLastState);
        if (m_isDownMovementKeyPressed)
        {
            m_keyDownTime += Time.deltaTime;
            if (m_keyDownTime > m_maxJumpHoldChargeTime)
            {
                m_keyDownTime = m_maxJumpHoldChargeTime;
            }
        }
    }

    public void OnDownMovementKeyStatusChange(bool keyStatus)
    {
        if (m_jumpingState == PlayerJumpingStates.idle && m_isDownMovementKeyPressed != keyStatus)
        {
            m_isDownMovementKeyPressed = keyStatus;
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
        m_anchorPosition = Vector3.zero;
        transform.localPosition = m_anchorPosition;

        m_jumpingState = PlayerJumpingStates.idle;
        m_playerIdleEvent.Raise();
        m_keyDownTime = 0;
        m_isDownMovementKeyPressed = false;
    }

    IEnumerator PlayerMoveUpCorutine()
    {
        m_jumpingState = PlayerJumpingStates.walking;
        m_playerWalkingEvent.Raise();
        InputReaderScript.s_instance.m_onUpMovementKeyPress -= OnUpMovementKeyPress;
        const float kVerticalTravelTimeSeconds = 1;
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
        //Since there is no climbing animation this is unnecessary and causes unwanted behaviour on the animations
        //m_playerIdleEvent.Raise();
    }

    [SerializeField]
    public AnimationCurve m_jumpingCurve;
    [SerializeField]
    float m_maxHorizontalJumpDistance = 10;
    [SerializeField]
    float m_maxVerticalJumpDistance = 10;
    [SerializeField]
    float m_maxJumpHoldChargeTime = 10;
    [SerializeField]
    float m_jumpFlyTime = 1;
    [SerializeField]
    Vector3 m_anchorPosition;
    IEnumerator PlayerArchingMoveDownCorutine(float keyPressTime)
    {
        m_jumpingState = PlayerJumpingStates.jumping;
        m_playerJumpingEvent.Raise();
        InputReaderScript.s_instance.m_onDownMovementKeyStatusChange -= OnDownMovementKeyStatusChange;

        var jumpVerticalDistance = Mathf.Max(keyPressTime, Mathf.Min(keyPressTime * keyPressTime, m_maxVerticalJumpDistance));
        var jumpHorizontalDistance = Mathf.Min(keyPressTime, m_maxHorizontalJumpDistance);
        m_anchorPosition = transform.localPosition;

        float accumulatedJumpTime = 0;
        do
        {
            accumulatedJumpTime += (Time.deltaTime);

            var normalizedTime = accumulatedJumpTime / m_jumpFlyTime;
            var horizontalPosition = jumpHorizontalDistance * m_jumpingCurve.Evaluate(normalizedTime);
            var verticalPosition = -jumpVerticalDistance * normalizedTime;

            var deltaPos = new Vector3(horizontalPosition, verticalPosition);
            transform.localPosition = m_anchorPosition + deltaPos;

            yield return null;
        } while (accumulatedJumpTime < m_jumpFlyTime);

        InputReaderScript.s_instance.m_onDownMovementKeyStatusChange += OnDownMovementKeyStatusChange;

        m_keyDownTime = 0;

        m_jumpingState = PlayerJumpingStates.idle;
        m_playerIdleEvent.Raise();
    }
}
