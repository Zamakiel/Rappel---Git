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
    int m_healthCurrent;

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
        //transform.localPosition += new Vector3(0, Time.deltaTime * m_moveSpeed, 0);
        GetComponent<Rigidbody2D>().linearVelocityY = m_moveSpeed;
        m_lifetimeCurrent += Time.deltaTime;
        if(m_lifetimeCurrent > m_lifetimeMax)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision on enemy");
    }
}
