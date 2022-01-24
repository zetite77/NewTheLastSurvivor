using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FROM The_last_Survivor_.xlsx
// 일반 NZOM
//라운드 1   2   3   4   5   6   7   8   9   10  11  12  13  14  15  16  17  18  19  20
//좀비 체력	10	30	50	70	90	110	130	150	170	190	210	230	250	270	290	310	330	350	370	390
//좀비 공격력	5	8	11	14	17	20	23	26	29	32	35	38	41	44	47	50	53	56	59	62
//좀비 이동 속도	0.5	0.75	1	1.25	1.5	1.75	2	2.25	2.5	2.75	3	3.25	3.5	3.75	4	4.25	4.5	4.75	5	5.25
//좀비 생성 속도	3	2.85	2.7	2.55	2.4	2.25	2.1	1.95	1.8	1.65	1.5	1.35	1.2	1.05	0.9	0.75	0.6	0.45	0.3	0.15
//좀비 공격 속도	2	1.97	1.94	1.91	1.88	1.85	1.82	1.79	1.76	1.73	1.7	1.67	1.64	1.61	1.58	1.55	1.52	1.49	1.46	1.43

// 특수 공격 AZOM
//라운드	3	4	5	6	7	8	9	10	11	12	13	14	15	16	17	18	19	20		
//좀비 체력	60	100	140	180	220	260	300	340	380	420	460	500	540	580	620	660	700	740		
//좀비 공격력	22	28	34	40	46	52	58	64	70	76	82	88	94	100	106	112	118	124		
//좀비 이동 속도	0.5	0.6	0.7	0.8	0.9	1	1.1	1.2	1.3	1.4	1.5	1.6	1.7	1.8	1.9	2	2.1	2.2	
//좀비 생성 속도	5	4.9	4.8	4.7	4.6	4.5	4.4	4.3	4.2	4.1	4	3.9	3.8	3.7	3.6	3.5	3.4	3.3	
//좀비 공격 속도	2.5	2.49	2.48	2.47	2.46	2.45	2.44	2.43	2.42	2.41	2.4	2.39	2.38	2.37	2.36	2.35	2.34	2.33		

// 특수 이속 SZOM
//라운드	3	4	5	6	7	8	9	10	11	12	13	14	15	16	17	18	19	20		
//좀비 체력	30	40	50	60	70	80	90	100	110	120	130	140	150	160	170	180	190	200		
//좀비 공격력	5	6.5	8	9.5	11	12.5	14	15.5	17	18.5	20	21.5	23	24.5	26	27.5	29	30.5		
//좀비 이동 속도	1	1.35	1.7	2.05	2.4	2.75	3.1	3.45	3.8	4.15	4.5	4.85	5.2	5.55	5.9	6.25	6.6	6.95		
//좀비 생성 속도	5	4.9	4.8	4.7	4.6	4.5	4.4	4.3	4.2	4.1	4	3.9	3.8	3.7	3.6	3.5	3.4	3.3		
//좀비 공격 속도	1.5	1.45	1.4	1.35	1.3	1.25	1.2	1.15	1.1	1.05	1	0.95	0.9	0.85	0.8	0.75	0.7	0.65		

public class Zombi : MonoBehaviour
{
    public int m_nHp = 10;
    public int m_nAtkDamage;
    public float m_fMoveSpeed;
    public float m_fResponedelay;
    public float m_fAtkdelay;

    public GameObject m_objCvsOnPlay;
    public GameObject m_objShalter;
    public GameObject m_objGrenade;
    public GameObject m_objUpgradeCanvas;
    public GameObject m_prefabDna;
    //public E_Zombi_State state = E_Zombi_State.NZOM;
    //public enum E_Zombi_State { NZOM, AZOM, SZOM}



    public bool m_bAttack = false;
    public bool m_bMove = true;

    // NZOM : Normal Zombie, AZOM : Attack Zombie, SZOM : Speed Zombie
    public int INIT_NZOM_HP = 10;
    public int INIT_NZOM_DMG = 5;
    public float INIT_NZOM_MOVE_SPEED = 0.5f;
    public float INIT_NZOM_RESP_DELAY = 3.0f;
    public float INIT_NZOM_ATK_DELAY = 2.0f;

    public int NZOM_HP_PER_LV = 20;
    public int NZOM_DMG_PER_LV = 3;
    public float NZOM_MOVE_SPEED_PER_LV = 0.25f;
    public float NZOM_RESP_DELAY_PER_LV = -0.15f;
    public float NZOM_ATK_DELAY_PER_LV = -0.03f;
    
    // --------AZOM--------
    public int INIT_AZOM_HP = 60;
    public int INIT_AZOM_DMG = 22;
    public float INIT_AZOM_MOVE_SPEED = 0.5f;
    public float INIT_AZOM_RESP_DELAY = 5.0f;
    public float INIT_AZOM_ATK_DELAY = 2.5f;

    public int AZOM_HP_PER_LV = 40;
    public int AZOM_DMG_PER_LV = 6;
    public float AZOM_MOVE_SPEED_PER_LV = 0.1f;
    public float AZOM_RESP_DELAY_PER_LV = -0.1f;
    public float AZOM_ATK_DELAY_PER_LV = -0.01f;
    // --------SZOM-------- 좀비 공격력 int라서 정수단위로 가야할듯?
    public int INIT_SZOM_HP = 30;
    public int INIT_SZOM_DMG = 5;
    public float INIT_SZOM_MOVE_SPEED = 1.0f;
    public float INIT_SZOM_RESP_DELAY = 5.0f;
    public float INIT_SZOM_ATK_DELAY = 1.5f;

    public int SZOM_HP_PER_LV = 10;
    public int SZOM_DMG_PER_LV = 2;
    public float SZOM_MOVE_SPEED_PER_LV = 0.35f;
    public float SZOM_RESP_DELAY_PER_LV = -0.1f;
    public float SZOM_ATK_DELAY_PER_LV = -0.05f;
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

            OnPlayScript.Instance.ShalterHpBar();
            if (GameManager.Instance.vibration)
            { // 진동 설정이 ON일 때
                Handheld.Vibrate();
            }
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

        ZombieLevelUp(OnPlayScript.Instance.numberOfStage);
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
        if (GameManager.Instance.m_objInGamePopupCanvas.activeSelf == true)
            Destroy(this.gameObject); // 레벨업, 게임오버, 게임클리어 시 싹쓸이
    }

    public void ZombieLevelUp(int stage)
    {
        int PerStage = 30 - stage;
        if (PerStage <= 4)
            PerStage = 4;
        int Rand = Random.Range(1, PerStage);
        // 3스테이지부터 특수좀비 등장
        if (stage >= 3 && Rand == 1)
        {
            m_nHp = INIT_AZOM_HP + AZOM_HP_PER_LV * (stage - 1);
            m_nAtkDamage = INIT_AZOM_DMG + AZOM_DMG_PER_LV * (stage - 1);
            m_fMoveSpeed = INIT_AZOM_MOVE_SPEED + AZOM_MOVE_SPEED_PER_LV * (stage - 1);
            m_fResponedelay = INIT_AZOM_RESP_DELAY + AZOM_RESP_DELAY_PER_LV * (stage - 1);
            m_fAtkdelay = INIT_AZOM_ATK_DELAY + AZOM_ATK_DELAY_PER_LV * (stage - 1);
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (stage >= 3 && Rand == 2)
        {
            m_nHp = INIT_SZOM_HP + SZOM_HP_PER_LV * (stage - 1);
            m_nAtkDamage = INIT_SZOM_DMG + SZOM_DMG_PER_LV * (stage - 1);
            m_fMoveSpeed = INIT_SZOM_MOVE_SPEED + SZOM_MOVE_SPEED_PER_LV * (stage - 1);
            m_fResponedelay = INIT_SZOM_RESP_DELAY + SZOM_RESP_DELAY_PER_LV * (stage - 1);
            m_fAtkdelay = INIT_SZOM_ATK_DELAY + SZOM_ATK_DELAY_PER_LV * (stage - 1);
            this.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            m_nHp = INIT_NZOM_HP + NZOM_HP_PER_LV * (stage - 1);
            m_nAtkDamage = INIT_NZOM_DMG + NZOM_DMG_PER_LV * (stage - 1);
            m_fMoveSpeed = INIT_NZOM_MOVE_SPEED + NZOM_MOVE_SPEED_PER_LV * (stage - 1);
            m_fResponedelay = INIT_NZOM_RESP_DELAY + NZOM_RESP_DELAY_PER_LV * (stage - 1);
            m_fAtkdelay = INIT_NZOM_ATK_DELAY + NZOM_ATK_DELAY_PER_LV * (stage - 1);
        }
              
        
    }
}
