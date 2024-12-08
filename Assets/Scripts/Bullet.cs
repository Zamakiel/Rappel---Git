using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_bulletLifeTime;
    public float m_bulletSpeed;

    Transform m_thisTransform;
    float m_time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_thisTransform = this.transform;
        m_time = 0f;
    }

    private void OnEnable()
    {
        m_time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_thisTransform.position = new Vector2(m_thisTransform.position.x - (m_bulletSpeed * Time.deltaTime), m_thisTransform.position.y);
        m_time += Time.deltaTime;
        if(m_time > m_bulletLifeTime)
        {
            this.gameObject.SetActive(false);
        }
    }
}
