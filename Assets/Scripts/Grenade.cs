using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Grenade : MonoBehaviour
{
    public GameObject m_objGrenadeOffBtn;
    public GameObject m_objGrenadeOnBtn;
    public GameObject m_objShelter;
    public GameObject m_objGrenade;
    public GameObject m_objZombi;

    public int m_nDamage = 200;
    public int m_nCount = 3;

    public bool istrigger;
    public bool m_bGrenade = false;

    public int GetCount() { return m_nCount; }
    public void SetCount(int count) { m_nCount = count; }
    public int GetDamage() { return m_nDamage; }


    public void Fire()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (m_objShelter != null)
            {
                this.gameObject.SetActive(false);
                transform.position = m_objShelter.transform.position;
                m_nCount = m_nCount - 1;
            }
            
        }
        
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseposition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 objposition = Camera.main.ScreenToWorldPoint(mouseposition);
            objposition.z = 0; 
            transform.position = objposition;
           
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            this.gameObject.SetActive(false);
        }

        if (this.gameObject.activeSelf == false)
            this.transform.position = m_objShelter.transform.position;
        if (m_objShelter == null)
        {
            m_nCount = 0;
        }
        if (m_bGrenade == false)
        {
            if (m_objGrenadeOffBtn.activeSelf == true)
            {
                transform.position = m_objShelter.transform.position;
                m_bGrenade = true;
            }
        }

        if (this.gameObject.activeSelf == false)
        {
            m_objGrenadeOnBtn.SetActive(true);
            m_objGrenadeOffBtn.SetActive(false);
        }
        
    }
}
