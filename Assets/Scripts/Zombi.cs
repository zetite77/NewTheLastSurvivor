﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour
{
    public int m_nHp = 300;
    public int m_nAtkDamage;
    public float m_fAtkdelay;
    public float m_fMoveSpeed;
    public float m_fResponedelay;


    public GameObject m_objCvsOnPlay;
    public GameObject m_objShalter;
    //public GameObject m_objCvsUpgrade;



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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shalter")
        {
            m_bMove = false;
        }

    }

    
    public void ZombiAttack()
    {
        m_fAtkdelay = m_fAtkdelay - Time.deltaTime;
        if (m_fAtkdelay <= 0)
        {
            m_bAttack = true;
            int ShalterHp = m_objShalter.GetComponent<ShalterInfo>().GetHp();
            ShalterHp = ShalterHp - m_nAtkDamage;
            m_objShalter.GetComponent<ShalterInfo>().SetHp(ShalterHp);
            Debug.Log(ShalterHp);
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
        m_objShalter = GameObject.FindGameObjectWithTag("Shalter");
        
    }

    void Update()
    {
       

        if (m_objShalter != null)
            ZombiMovement();
        if (m_nHp <= 0)
            Destroy(this.gameObject);

            
        if (m_objCvsOnPlay.activeSelf == false)
            Destroy(this.gameObject);


    }
}
