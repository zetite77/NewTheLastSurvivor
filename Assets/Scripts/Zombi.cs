using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour
{
    public int m_nHp = 10;
    public int m_nAtkDamage;
    public float m_fAtkdelay;
    public float m_fMoveSpeed;
    public float m_fResponedelay;


    public GameObject m_objCvsOnPlay;
    public GameObject m_objShalter;
    public GameObject m_objGrenade;
    public GameObject m_objUpgradeCanvas;
    public GameObject m_prefabDna;



    public bool m_bAttack = false;
    public bool m_bMove = true;


    #region 세터게터
    public int GetHP ()                      { return m_nHp; }
    public void SetHP (int Hp)               { m_nHp = Hp; }
    public int GetDamage()                   { return m_nAtkDamage; }
    public void SetDamage (int Damage)       { m_nAtkDamage = Damage; }
    public float GetAtkSpeed ()              { return m_fAtkdelay; }
    public void SetAtkSpeed (float AtkSpeed) { m_fAtkdelay = AtkSpeed; }
    public float GetMoveSpeed ()             { return m_fMoveSpeed; }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shalter")
        {
            m_bMove = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grenade")
        {
            if (m_objGrenade.activeSelf == false)
            {
                m_nHp = m_nHp - m_objGrenade.GetComponent<Grenade>().m_nDamage;
            }
        }
        
    }


    public void ZombiAttack()
    {
        m_fAtkdelay = m_fAtkdelay - Time.deltaTime;
        if (m_fAtkdelay <= 0)
        {
            m_bAttack = true;
            int ShalterHp = m_objShalter.GetComponent<ShalterInfo>().GetHp();
            int ShalterDef = m_objShalter.GetComponent<ShalterInfo>().m_nDef;
            ShalterHp = ShalterHp - (m_nAtkDamage-ShalterDef);
            m_objShalter.GetComponent<ShalterInfo>().SetHp(ShalterHp);
            m_fAtkdelay = 3.0f;
        }
    }
    public void ZombiMovement()
    {
        if (m_objCvsOnPlay.activeSelf == true)
        {
            if (m_bMove == true)
            {
                Vector2 target = new Vector2(m_objShalter.transform.position.x, m_objShalter.transform.position.y);
                this.transform.position = Vector2.MoveTowards(this.transform.position, target, m_fMoveSpeed * Time.deltaTime);
            }
            else 
            {
                ZombiAttack();
            }
        }

    }
    void Start()
    {
        m_objCvsOnPlay = GameObject.FindGameObjectWithTag("Cvs_OnPlay");
        m_objShalter = GameManager.Instance.m_objShalter;
        m_objGrenade = GameManager.Instance.m_objGrenade;
        m_objUpgradeCanvas = GameManager.Instance.m_objUpgradeCanvas;
        
    }

    void Update()
    {
        if (m_objShalter != null)
            ZombiMovement();

        if (m_nHp <= 0)
        {
            Instantiate<GameObject>(m_prefabDna, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            //OnPlayScript.Instance.userDNA += 5;
            
        }
        if (m_objUpgradeCanvas.activeSelf == true)
            Destroy(this.gameObject);
            
        if (m_objCvsOnPlay.activeSelf == false)
            Destroy(this.gameObject);


        
    }
}
