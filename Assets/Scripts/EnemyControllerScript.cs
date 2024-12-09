using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControllerScript : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed;
    [SerializeField]
    int m_healthMax;
    [SerializeField]
    int m_healthCurrentPrivate;
    int m_healthCurrent
    {
        get { return m_healthCurrentPrivate; }
        set
        {
            m_healthCurrentPrivate = value;
            if (m_healthCurrentPrivate <= 0)
            {
                TerminateEnemy();
            }
        }
    }

    [SerializeField]
    int m_attackDamage;

    [SerializeField]
    float m_lifetimeMax;
    [SerializeField]
    float m_lifetimeCurrent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(0, Time.deltaTime * m_moveSpeed, 0);
        m_lifetimeCurrent += Time.deltaTime;
        if (m_lifetimeCurrent > m_lifetimeMax)
        {
            TerminateEnemy();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.name.Contains("Roof"))
            {
                PlayerScript.s_instance.m_onDealDamageToPlayerEvent.Invoke(m_attackDamage);
                TerminateEnemy();
            }

            if (collision.gameObject.name.Contains("Bullet"))
            {
                m_healthCurrent -= collision.gameObject.GetComponent<Bullet>().m_damage;

            }
        }
    }

    public void TerminateEnemy()
    {
        Destroy(gameObject);
    }
}
