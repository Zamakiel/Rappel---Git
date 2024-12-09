using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_bulletLifeTime;
    public float m_bulletSpeed;

    [SerializeField]
    public int m_damage;

    Transform m_thisTransform;
    float m_time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_thisTransform = this.transform;
        m_time = 0f;
    }

    void Awake()
    {
        m_time = 0f;
        m_damage = 3;
    }

    // Update is called once per frame
    void Update()
    {
        m_thisTransform.position = new Vector2(m_thisTransform.position.x - (m_bulletSpeed * Time.deltaTime), m_thisTransform.position.y);
        m_time += Time.deltaTime;
        if (m_time > m_bulletLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
