using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiRespone : MonoBehaviour
{
    public GameObject m_prefabZombi;
    public GameObject m_objShalter;
    public GameObject m_objCvsUpgrade;

    public float m_fResTime;

    public bool m_bRespone;

    public void Respone()
    {
        m_fResTime = m_fResTime - Time.deltaTime;
        Debug.Log(m_fResTime);
        if (m_fResTime <= 0)
        {

            m_bRespone = true;
            Instantiate<GameObject>(m_prefabZombi, this.transform);
            m_fResTime = 3.0f;
                
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (m_objShalter != null)
        {
            if(m_objCvsUpgrade.activeSelf != true)
                Respone();
        }
    }
}
