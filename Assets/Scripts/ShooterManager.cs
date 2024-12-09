using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

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

    void Awake()
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

        if (!m_initialized) { Initialize(); }
    }

    public bool m_initialized = false;
    public void Initialize()
    {
        m_canShoot = true;
        m_gunPoint = PlayerScript.s_instance.transform.Find("Gunpoint").transform;

        m_initialized = true;
        Debug.Log(this.GetType().ToString() + " Initialized!");
    }

    public void Shoot()
    {
        if (m_canShoot)
        {
            m_canShoot = false;
            GameObject l_bullet = Instantiate(m_bulletPrefab, m_gunPoint.position, Quaternion.identity);
            StartCoroutine(GunCooldown());
        }
    }

    IEnumerator GunCooldown()
    {
        yield return new WaitForSeconds(m_shootingInterval);
        m_canShoot = true;
    }
}
