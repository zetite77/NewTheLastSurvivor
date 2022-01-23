using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    enum UPGRADE_SELECT
    {
        SHELTER_DEF = 0, SHELTER_HP, WEAPON_DAMAGE,
        ATTACK_SPEED, RELOAD_SPEED, BUY_GRENADE, MAX_UPGRADE_SELECT = 6
    }

    public Button[] m_BtnUpgradeList = new Button[(int)UPGRADE_SELECT.MAX_UPGRADE_SELECT];
    public Text[] TxtUpgradeList = new Text[(int)UPGRADE_SELECT.MAX_UPGRADE_SELECT];
    private int[] UpgradeNum = new int[(int)UPGRADE_SELECT.MAX_UPGRADE_SELECT];
    const int MAX_UPGRADE = 10;
    public Button m_Btn_Exit;

    public float ATTACK_SPEED_PER_UPGRADE = 0.02f;
    public float RELOAD_SPEED_PER_UPGRADE = 0.02f;
    public int DAMAGE_PER_UPGRADE = 15;

    void Start()
    {
        //for (UPGRADE_SELECT i = 0; i < UPGRADE_SELECT.MAX_UPGRADE_SELECT; i++)
        //{
        //    m_BtnUpgradeList[(int)i].onClick.AddListener(() => ApplyUpgrade(i));

        //} 반복문 이거안됨

        m_BtnUpgradeList[(int)UPGRADE_SELECT.SHELTER_DEF].onClick.AddListener(() => ApplyUpgrade(UPGRADE_SELECT.SHELTER_DEF));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.SHELTER_HP].onClick.AddListener(() => ApplyUpgrade(UPGRADE_SELECT.SHELTER_HP));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.WEAPON_DAMAGE].onClick.AddListener(() => ApplyUpgrade(UPGRADE_SELECT.WEAPON_DAMAGE));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.ATTACK_SPEED].onClick.AddListener(() => ApplyUpgrade(UPGRADE_SELECT.ATTACK_SPEED));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.RELOAD_SPEED].onClick.AddListener(() => ApplyUpgrade(UPGRADE_SELECT.RELOAD_SPEED));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.BUY_GRENADE].onClick.AddListener(() => ApplyUpgrade(UPGRADE_SELECT.BUY_GRENADE));
        m_Btn_Exit.onClick.AddListener(() => GameManager.Instance.GameOver("asdf", 7, 89));

        OnPlayScript.Instance.m_TxtUserDNA.text = OnPlayScript.Instance.userDNA.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnEnable()
    {// 업글창이 뜨면 재장전
        GunManager.Instance.remainBullet = 
            GunManager.Instance.gunList[GunManager.Instance.currentGunPtr].BulletMaxSize;
        GunManager.Instance.m_TxtRemainBullet.text = GunManager.Instance.remainBullet.ToString();
    }

   
    void ApplyUpgrade(UPGRADE_SELECT upgradeSelect)
    {
        switch (upgradeSelect)
        {
            case UPGRADE_SELECT.SHELTER_DEF:
                if (OnPlayScript.Instance.userDNA >= 20)
                {
                    UpgradeButtonControl(upgradeSelect, 20);
                    GameManager.Instance.m_objShalter.GetComponent<ShalterInfo>().m_nDef++;
                }
                break;
            case UPGRADE_SELECT.SHELTER_HP:
                if (OnPlayScript.Instance.userDNA >= 20)
                {
                    UpgradeButtonControl(upgradeSelect, 20);
                    GameManager.Instance.m_objShalter.GetComponent<ShalterInfo>().m_nHp++;
                }
                break;
            case UPGRADE_SELECT.WEAPON_DAMAGE:
                if (OnPlayScript.Instance.userDNA >= 20) UpgradeButtonControl(upgradeSelect, 20);
                for (int idx = 0; idx < GunManager.Instance.gunList.Length; idx++)
                { // 총기 종류마다 데미지 싹 다 오름
                    GunManager.Instance.gunList[idx].damage += DAMAGE_PER_UPGRADE;

                }
                break;
            case UPGRADE_SELECT.ATTACK_SPEED:
                if (OnPlayScript.Instance.userDNA >= 10) UpgradeButtonControl(upgradeSelect, 10);
                for (int idx = 0; idx < GunManager.Instance.gunList.Length; idx++)
                {
                    GunManager.Instance.gunList[idx].attackSpeed -= ATTACK_SPEED_PER_UPGRADE;

                }
                break;
            case UPGRADE_SELECT.RELOAD_SPEED:
                if (OnPlayScript.Instance.userDNA >= 10) UpgradeButtonControl(upgradeSelect, 10);
                for (int idx = 0; idx < GunManager.Instance.gunList.Length; idx++)
                {
                    GunManager.Instance.gunList[idx].reloadSpeed -= RELOAD_SPEED_PER_UPGRADE;

                }
                break;
            case UPGRADE_SELECT.BUY_GRENADE:
                if (OnPlayScript.Instance.userDNA >= 35) UpgradeButtonControl(upgradeSelect, 35);
                break;
            case UPGRADE_SELECT.MAX_UPGRADE_SELECT:
                break;
            default:
                Debug.Log(this + "Upgrade Err");
                break;
        }
        OnPlayScript.Instance.m_TxtUserDNA.text = OnPlayScript.Instance.userDNA.ToString();
    }

    void UpgradeButtonControl(UPGRADE_SELECT upgradeSelect, int needDna)
    {
        OnPlayScript.Instance.userDNA -= needDna;
        UpgradeNum[(int)upgradeSelect]++;
        TxtUpgradeList[(int)upgradeSelect].text =
            "+" + UpgradeNum[(int)upgradeSelect].ToString();
        if (UpgradeNum[(int)upgradeSelect] == MAX_UPGRADE)
            m_BtnUpgradeList[(int)upgradeSelect].interactable = false;
    }
}
