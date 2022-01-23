using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiRespone : MonoBehaviour
{
    public GameObject m_prefabZombi;
    public GameObject m_objShalter;
    public GameObject m_objCvsUpgrade;
    public GameObject m_objCvsTitle;
    public GameObject m_objCvsOnPlay;

    public float m_fResTime;
    public int testCount;
    public bool m_bRespone;

    public void Respone()
    {
        m_fResTime = m_fResTime - Time.deltaTime;
        if (m_fResTime <= 0)
        {

            m_bRespone = true;
            Instantiate<GameObject>(m_prefabZombi, this.transform);
            //m_fResTime = 3.0f;
            m_fResTime = 1.0f;

        }
    }

    // Update is called once per frame
        
    
    void Update()
    {
            Transform[] childlist = gameObject.GetComponentsInChildren<Transform>();
        if (m_objShalter != null)
        {
            if (m_objCvsUpgrade.activeSelf != true)
                Respone();
            else
                m_fResTime = 3.0f;
        }
        if (m_objCvsTitle.activeSelf == true)
        {
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
