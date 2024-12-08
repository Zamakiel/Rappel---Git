using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ShooterManager : MonoBehaviour
{
    public static ShooterManager s_instance;

    public Transform m_gunPoint;
    public GameObject m_bulletPrefab;
    public GameObject m_bulletPrefab2;

    public float m_shootingInterval;

    public List<GameObject> m_bulletPool;

    bool m_canShoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        m_canShoot = true;
    }

    public void Shoot()
    {
        if (m_canShoot)
        {
            m_canShoot = false;
            StartCoroutine(DebounceBullets());

            if (m_bulletPool.Exists(x => !x.activeInHierarchy))
            {
                GameObject l_bullet = m_bulletPool.Find(x => !x.gameObject.activeInHierarchy);
                l_bullet.transform.position = m_gunPoint.position;
                l_bullet.SetActive(true);
            }
            else
            {
                GameObject l_bullet = Instantiate(m_bulletPrefab, m_gunPoint.position, Quaternion.identity);
                m_bulletPool.Add(l_bullet);
            }
        }
    }

    IEnumerator DebounceBullets()
    {
        yield return new WaitForSeconds(m_shootingInterval);
        m_canShoot = true;
    }
}
