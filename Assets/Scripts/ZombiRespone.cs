using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiRespone : MonoBehaviour
{
    public GameObject m_prefabZombi;
    public GameObject m_objShalter;
    public GameObject m_objCvsUpgrade;
    public GameObject m_objCvsTitle;

    public float m_fResTime;

    public bool m_bRespone;

    public void Respone()
    {
        m_fResTime = m_fResTime - Time.deltaTime;
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
            if (m_objCvsUpgrade.activeSelf != true)
                Respone();
            else
                m_fResTime = 3.0f;
        }
        if (m_objCvsTitle.activeSelf == true)
        {
            Transform[] childlist = gameObject.GetComponentsInChildren<Transform>();
            if(childlist != null)
            {
                for(int i = 1; i < childlist.Length; i++)
                {
                    Destroy(childlist[i].gameObject);
                }
            }

            this.gameObject.SetActive(false);
            m_fResTime = 3.0f;
        }


    }
}
