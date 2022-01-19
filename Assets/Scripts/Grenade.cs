using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Grenade : MonoBehaviour
{
    public GameObject m_objGrenadeOffBtn;
    public GameObject m_objGrenadeOnBtn;
    public GameObject m_objTempShelter;
    public GameObject m_objGrenade;
    public GameObject m_objZombi;

    public int m_nDamage = 200;
    public int m_nCount = 3;

    public bool istrigger;
    public bool m_bGrenade = false;

    public int GetCount() { return m_nCount; }
    public void SetCount(int count) { m_nCount = count; }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Zombi")
            istrigger = true;
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Zombi")
            istrigger = false;
    }
    public void Fire()
    {

        if (Input.GetMouseButtonUp(0))
        {

            if (istrigger == true)
            {
                Debug.Log("충돌");
                transform.position = m_objTempShelter.transform.position;
                this.gameObject.SetActive(false);
                Zombi zombi = m_objZombi.GetComponent<Zombi>();
                int Hp = zombi.GetHP();
                Hp = Hp - m_nDamage;
                zombi.SetHP(Hp);
                Debug.Log("남은 좀비 Hp : " + zombi.GetHP());
                istrigger = false;
                m_nCount = m_nCount - 1;
            }
            else
            {
                this.gameObject.SetActive(false);
                transform.position = m_objTempShelter.transform.position;
                m_nCount = m_nCount - 1;
            }
            
        }
        
    }
    void Update()
    {

        if (m_bGrenade == false)
        {
            if (m_objGrenadeOffBtn.activeSelf == true)
            {
                transform.position = m_objTempShelter.transform.position;
                m_bGrenade = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseposition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 objposition = Camera.main.ScreenToWorldPoint(mouseposition);
            objposition.z = 0; 
            transform.position = objposition;
        }
            Fire();

        if (this.gameObject.activeSelf == false)
        {
            m_objGrenadeOnBtn.SetActive(true);
            m_objGrenadeOffBtn.SetActive(false);
        }
        //if (m_nCount <= 0)
        //{
        //    m_objGrenadeOnBtn.SetActive(false);
        //    m_objGrenadeOffBtn.SetActive(true);
        //}

        
    }
}
