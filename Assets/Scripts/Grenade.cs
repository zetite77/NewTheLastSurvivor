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
    public GameObject m_objCvsOnPlay;
    public GameObject m_objUpgradeCanvas;
    public GameObject m_objGameManager;

    public Text m_GrenadeOffBtnText;
    public Text m_GrenadeOnBtnText;
    public AudioSource m_GrenadeExplosion;

    public bool m_bfire;

    public int m_nDamage = 200;
    public int m_nCount;

    public bool istrigger;
    public bool m_bGrenade = false;

    public int GetDamage() { return m_nDamage; }


    void Start()
    {
        m_objUpgradeCanvas = GameManager.Instance.m_objUpgradeCanvas;
    }
    void Update()
    {
        
        Vector2 mouseposition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 objposition = Camera.main.ScreenToWorldPoint(mouseposition);

        if(m_objCvsOnPlay)
        m_nCount = m_objGameManager.GetComponent<GameManager>().m_nGrenadeCount;
        
        if (m_objUpgradeCanvas.activeSelf == true)
            this.gameObject.SetActive(true);
        if (Input.GetMouseButton(0))
        {
            transform.position = objposition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_bfire = true;
            m_objCvsOnPlay.GetComponent<GunManager>().enabled = true;
            this.gameObject.SetActive(false);
            m_nCount = m_nCount - 1;
            m_objGameManager.GetComponent<GameManager>().m_nGrenadeCount = m_nCount;

            m_GrenadeExplosion.Play();
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
        if (m_nCount <= 0)
            m_nCount = 0;
        else
            m_GrenadeOffBtnText.text = (m_nCount - 1).ToString();

        m_GrenadeOnBtnText.text = m_nCount.ToString();
        
    }
}
