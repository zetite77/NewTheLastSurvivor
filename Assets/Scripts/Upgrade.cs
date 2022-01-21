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

    public Text m_TxtUserDNA;
    public int userDNA;

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

        m_TxtUserDNA.text = userDNA.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ApplyUpgrade(UPGRADE_SELECT upgradeSelect)
    {
        switch (upgradeSelect)
        {
            case UPGRADE_SELECT.SHELTER_DEF:
                if (userDNA >= 20) UpgradeButtonControl(upgradeSelect, 20);
                break;
            case UPGRADE_SELECT.SHELTER_HP:
                if (userDNA >= 20) UpgradeButtonControl(upgradeSelect, 20);
                break;
            case UPGRADE_SELECT.WEAPON_DAMAGE:
                if (userDNA >= 20) UpgradeButtonControl(upgradeSelect, 20);
                break;
            case UPGRADE_SELECT.ATTACK_SPEED:
                if (userDNA >= 10) UpgradeButtonControl(upgradeSelect, 10);
                break;
            case UPGRADE_SELECT.RELOAD_SPEED:
                if (userDNA >= 10) UpgradeButtonControl(upgradeSelect, 10);
                break;
            case UPGRADE_SELECT.BUY_GRENADE:
                if (userDNA >= 35) UpgradeButtonControl(upgradeSelect, 35);
                break;
            case UPGRADE_SELECT.MAX_UPGRADE_SELECT:
            default:
                Debug.Log(this + "Upgrade Err");
                break;
        }
        m_TxtUserDNA.text = userDNA.ToString();
    }

    void UpgradeButtonControl(UPGRADE_SELECT upgradeSelect, int needDna)
    {
        userDNA -= needDna;
        UpgradeNum[(int)upgradeSelect]++;
        TxtUpgradeList[(int)upgradeSelect].text =
            "+" + UpgradeNum[(int)upgradeSelect].ToString();
        if (UpgradeNum[(int)upgradeSelect] == MAX_UPGRADE)
            m_BtnUpgradeList[(int)upgradeSelect].interactable = false;
    }
}
