using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript s_instance;

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
    GameEvent m_onChargeStart;
    [SerializeField]
    GameEvent m_onChargeEnd;

    [SerializeField]
    GameObject m_playeerGraphics;
    SpriteRenderer m_spriteRenderer;

    [SerializeField]
    int m_playerHealthCurrent;
    [SerializeField]
    int m_playerHealthMax;

    public delegate void OnDealDamageToPlayerEvent(int damage);
    public OnDealDamageToPlayerEvent m_onDealDamageToPlayerEvent;

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
        DontDestroyOnLoad(this);

        if (!m_initialized)
        {
            Initialize();
        }
    }

    public bool m_initialized = false;
    public void Initialize()
    {
        m_isDownMovementKeyPressed = false;
        m_jumpingState = PlayerJumpingStates.idle;
        m_keyDownTime = 0;
        m_maxYPosition = 2.5f;

        m_maxHorizontalJumpDistance = 10;
        m_maxVerticalJumpDistance = 10;
        m_maxJumpHoldChargeTime = 10;
        m_jumpFlyTime = 1;
        m_startPosition = Vector3.zero;
        m_anchorPosition = Vector3.zero;

        m_playerHealthMax = 10;
        m_playerHealthCurrent = m_playerHealthMax;
        m_onDealDamageToPlayerEvent += ReceiveDamage;

        m_spriteRenderer = m_playeerGraphics.GetComponent<SpriteRenderer>();

        ResetPlayer();

        m_initialized = true;
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    float m_currentRappelTime;
    float m_maxRappelTimeSeconds;

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

        if (m_jumpingState == PlayerJumpingStates.idle)
        {
            if (transform.localPosition != m_anchorPosition)
            {
                m_currentRappelTime += Time.deltaTime;
            }
            if (m_currentRappelTime > m_maxRappelTimeSeconds)
            {
                m_currentRappelTime = 0;
                StartCoroutine(PlayerMoveUpCorutine());
            }
        }
    }

    public void OnDownMovementKeyStatusChange(bool keyStatus)
    {
        if (m_jumpingState == PlayerJumpingStates.idle && m_isDownMovementKeyPressed != keyStatus)
        {
            m_isDownMovementKeyPressed = keyStatus;

            if (keyStatus)
            {
                m_onChargeStart.Raise();
            }

            if (!m_isDownMovementKeyPressed && m_jumpingState == PlayerJumpingStates.idle)
            {
                m_onChargeEnd.Raise();
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
        GameObject lastFloorGameObject = WorldManagerScript.s_instance.m_entireBuildingOrderedGameObjects[WorldManagerScript.s_instance.m_entireBuildingOrderedGameObjects.Count - 2];
        SpriteRenderer lastFloorSprite = WorldManagerScript.s_instance.m_buildingRoofGameObject.GetComponent<SpriteRenderer>();

        m_startPosition = lastFloorGameObject.transform.position;
        m_startPosition.x += 0.5f * lastFloorGameObject.transform.localScale.x * lastFloorSprite.sprite.rect.size.x / lastFloorSprite.sprite.pixelsPerUnit;
        m_startPosition.x += 0.5f * m_playeerGraphics.transform.localScale.x * m_spriteRenderer.sprite.rect.size.x / m_spriteRenderer.sprite.pixelsPerUnit;
        transform.localPosition = m_startPosition;
        m_anchorPosition = m_startPosition;

        m_jumpingState = PlayerJumpingStates.idle;
        m_playerIdleEvent.Raise();
        m_keyDownTime = 0;
        m_isDownMovementKeyPressed = false;
        m_currentRappelTime = 0;
        m_maxRappelTimeSeconds = 50;
    }

    IEnumerator PlayerMoveUpCorutine()
    {
        InputReaderScript.s_instance.m_onUpMovementKeyPress -= OnUpMovementKeyPress;
        //Serialize this?
        const float kVerticalTravelTimeSeconds = 1;
        const float kHorizontalTravelTimeSeconds = 1;
        m_anchorPosition = m_startPosition;

        if (m_isDownMovementKeyPressed)
        {
            m_isDownMovementKeyPressed = false;
        }
        if (m_keyDownTime != 0)
        {
            m_keyDownTime = 0;
        }

        m_jumpingState = PlayerJumpingStates.walking;
        var travelDistance = m_anchorPosition.x - transform.localPosition.x;
        while (transform.localPosition.x != m_anchorPosition.x)
        {
            var deltaMove = new Vector3(Time.deltaTime * travelDistance / kHorizontalTravelTimeSeconds, 0, 0);
            if (transform.localPosition.x + deltaMove.x < m_anchorPosition.x)
            {
                deltaMove.x = m_anchorPosition.x - transform.localPosition.x;
            }
            transform.localPosition += deltaMove;
            yield return null;
        };

        m_jumpingState = PlayerJumpingStates.climbing;
        travelDistance = m_anchorPosition.y - transform.localPosition.y;
        while (transform.localPosition.y != m_anchorPosition.y)
        {
            var deltaMove = new Vector3(0, Time.deltaTime * travelDistance / kVerticalTravelTimeSeconds, 0);
            if (transform.localPosition.y + deltaMove.y > m_anchorPosition.y)
            {
                deltaMove.y = m_anchorPosition.y - transform.localPosition.y;
            }
            transform.localPosition += deltaMove;
            yield return null;
        };

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
    Vector3 m_startPosition;
    [SerializeField]
    Vector3 m_anchorPosition;
    IEnumerator PlayerArchingMoveDownCorutine(float keyPressTime)
    {
        m_jumpingState = PlayerJumpingStates.jumping;
        m_currentRappelTime = 0;
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

            if (transform.position.y <= WorldManagerScript.s_instance.m_buildingBaseGameObject.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, WorldManagerScript.s_instance.m_buildingBaseGameObject.transform.position.y, transform.position.z);
                StartCoroutine(PlayerMoveUpCorutine());
            }

            yield return null;
        } while (accumulatedJumpTime < m_jumpFlyTime && transform.position.y > WorldManagerScript.s_instance.m_buildingBaseGameObject.transform.position.y);

        InputReaderScript.s_instance.m_onDownMovementKeyStatusChange += OnDownMovementKeyStatusChange;

        m_keyDownTime = 0;

        m_jumpingState = PlayerJumpingStates.idle;
        m_playerIdleEvent.Raise();
    }

    public void ReceiveDamage(int damageAmount)
    {
        m_playerHealthCurrent -= damageAmount;
    }
}
