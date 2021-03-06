using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    #region 싱글톤구현
    private static Upgrade _instance;
    public static Upgrade Instance
    {
        get
        {
            if (!_instance)
            {// 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                _instance = FindObjectOfType(typeof(Upgrade)) as Upgrade;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    #endregion
    enum UPGRADE_SELECT
    {
        SHELTER_DEF = 0, SHELTER_HP, WEAPON_DAMAGE,
        RELOAD_SPEED, ATTACK_SPEED, BUY_GRENADE, MAX_UPGRADE_SELECT = 6
    }
    // OnPlay와 같음
    public GameObject m_objGrenade;
    public Image m_ProgBar_NightTime;
    public Text m_TxtStage;
    public Text m_TxtUserDNA;
    const float PROGRESS_MAX = 1.0f; // 프로그래스 바 게이지 최대치(100%)
    const float PROGRESS_MIN = 0.0f;

    public Button[] m_BtnUpgradeList = new Button[(int)UPGRADE_SELECT.MAX_UPGRADE_SELECT];
    public Text[] TxtUpgradeList = new Text[(int)UPGRADE_SELECT.MAX_UPGRADE_SELECT];
    public int[] UpgradeNum = new int[(int)UPGRADE_SELECT.MAX_UPGRADE_SELECT];
    public Text[] TxtUpgradeCost = new Text[(int)UPGRADE_SELECT.MAX_UPGRADE_SELECT];
    const int MAX_UPGRADE = 9;
    public Button m_Btn_Exit;

    // 상?수 입니다만, 모니터링하며 값을 바꿔야하기 때문에 퍼블릭변수화
    public float ATTACK_SPEED_PER_UPGRADE = 0.046f;
    public float RELOAD_SPEED_PER_UPGRADE = 0.065f;
    public int INIT_DAMAGE_PER_UPGRADE = 15;
    public int DAMAGE_PER_UPGRADE = 15;
    public int SHELTER_DEF_PER_UPGRADE = 7;
    public int SHELTER_HP_PER_UPGRADE = 120;

    public int SHELTER_DEF_UP_COST = 20;
    public int SHELTER_HP_UP_COST = 20;
    public int DAMAGE_UP_COST = 10;
    public int ATTACK_SPEED_UP_COST = 10;
    public int RELOAD_SPEED_UP_COST = 10;
    public int GRENADE_UP_COST = 35;


    void Start()
    {
        //for (UPGRADE_SELECT i = 0; i < UPGRADE_SELECT.MAX_UPGRADE_SELECT; i++)
        //{
        //    m_BtnUpgradeList[(int)i].onClick.AddListener(() => ApplyUpgrade(i));

        //} 반복문 이거안됨

        m_BtnUpgradeList[(int)UPGRADE_SELECT.SHELTER_DEF].onClick.AddListener
            (() => ApplyUpgrade(UPGRADE_SELECT.SHELTER_DEF));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.SHELTER_HP].onClick.AddListener
            (() => ApplyUpgrade(UPGRADE_SELECT.SHELTER_HP));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.WEAPON_DAMAGE].onClick.AddListener
            (() => ApplyUpgrade(UPGRADE_SELECT.WEAPON_DAMAGE));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.ATTACK_SPEED].onClick.AddListener
            (() => ApplyUpgrade(UPGRADE_SELECT.ATTACK_SPEED));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.RELOAD_SPEED].onClick.AddListener
            (() => ApplyUpgrade(UPGRADE_SELECT.RELOAD_SPEED));
        m_BtnUpgradeList[(int)UPGRADE_SELECT.BUY_GRENADE].onClick.AddListener
            (() => ApplyUpgrade(UPGRADE_SELECT.BUY_GRENADE));   
        m_Btn_Exit.onClick.AddListener(() => GameManager.Instance.RankUpload("K-ookbob",
            OnPlayScript.Instance.numberOfStage, GunManager.Instance.zombieKills));

        OnPlayScript.Instance.m_TxtUserDNA.text = OnPlayScript.Instance.userDNA.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        // 초당 1 / duration 만큼 부드럽게(매 프레임 마다) 증가 
        // duration이 15일 경우,         0초 일 때 1  ->  7.5초 일 때 0.5  ->  15초 일 때 0
        m_ProgBar_NightTime.fillAmount = OnPlayScript.Instance.m_ProgBar_NightTime.fillAmount;
        m_TxtUserDNA.text = OnPlayScript.Instance.m_TxtUserDNA.text;
        for (int i = 0; i < m_BtnUpgradeList.Length; i++)
        {
            switch (i)
            {
                case 0:
                    TxtUpgradeCost[i].text = SHELTER_DEF_UP_COST.ToString();
                    break;
                case 1:
                    TxtUpgradeCost[i].text = SHELTER_HP_UP_COST.ToString();
                    break;
                case 2:
                    TxtUpgradeCost[i].text = DAMAGE_UP_COST.ToString();
                    break;
                case 3:
                    TxtUpgradeCost[i].text = RELOAD_SPEED_UP_COST.ToString();
                    break;
                case 4:
                    TxtUpgradeCost[i].text = ATTACK_SPEED_UP_COST.ToString();
                    break;
                case 5:
                    TxtUpgradeCost[i].text = GRENADE_UP_COST.ToString();
                    break;
            }
        }
    }

    private void OnEnable()
    {// 업글창이 뜨면 재장전
        GunManager.Instance.remainBullet =
            GunManager.Instance.gunList[GunManager.Instance.currentGunPtr].BulletMaxSize;
        GunManager.Instance.m_TxtRemainBullet.text = GunManager.Instance.remainBullet.ToString();

        m_TxtStage.text = OnPlayScript.Instance.m_Text_StageTxt.text;

        if (OnPlayScript.Instance.initFlg)
            InitStats();


        // 업글창이 뜨면 텍스트들 초기화
        TxtUpgradeList[(int)UPGRADE_SELECT.BUY_GRENADE].text =
            GameManager.Instance.m_nGrenadeCount.ToString();
    }

    void InitStats()
    {
        SHELTER_DEF_UP_COST = 20;
        SHELTER_HP_UP_COST = 20;
        DAMAGE_UP_COST = 10;
        ATTACK_SPEED_UP_COST = 10;
        RELOAD_SPEED_UP_COST = 10;

        TxtUpgradeList[(int)UPGRADE_SELECT.SHELTER_DEF].text = "0";
        TxtUpgradeList[(int)UPGRADE_SELECT.SHELTER_HP].text = "0";
        TxtUpgradeList[(int)UPGRADE_SELECT.WEAPON_DAMAGE].text = "0";
        TxtUpgradeList[(int)UPGRADE_SELECT.ATTACK_SPEED].text = "1";
        TxtUpgradeList[(int)UPGRADE_SELECT.RELOAD_SPEED].text = "1";
        TxtUpgradeList[(int)UPGRADE_SELECT.BUY_GRENADE].text = 
            GameManager.Instance.m_nGrenadeCount.ToString();

        OnPlayScript.Instance.initFlg = false;
    }

    void ApplyUpgrade(UPGRADE_SELECT upgradeSelect)
    {
        switch (upgradeSelect)
        {
            case UPGRADE_SELECT.SHELTER_DEF:
                if (OnPlayScript.Instance.userDNA >= SHELTER_DEF_UP_COST)
                {
                    UpgradeButtonControl(upgradeSelect, SHELTER_DEF_UP_COST, SHELTER_DEF_PER_UPGRADE);
                    SHELTER_DEF_UP_COST = SHELTER_DEF_UP_COST + 20;
                    GameManager.Instance.m_objShalter.GetComponent<ShalterInfo>()
                        .m_nDef += SHELTER_DEF_PER_UPGRADE;

                }
                break;
            case UPGRADE_SELECT.SHELTER_HP:
                if (OnPlayScript.Instance.userDNA >= SHELTER_HP_UP_COST)
                {
                    UpgradeButtonControl(upgradeSelect, SHELTER_HP_UP_COST, SHELTER_HP_PER_UPGRADE);
                    SHELTER_HP_UP_COST = SHELTER_HP_UP_COST + 20;
                    GameManager.Instance.m_objShalter.GetComponent<ShalterInfo>()
                        .m_nInitHp += SHELTER_HP_PER_UPGRADE;

                }
                break;
            case UPGRADE_SELECT.WEAPON_DAMAGE:
                if (OnPlayScript.Instance.userDNA >= DAMAGE_UP_COST)
                { // 첫 업글은 뎀 +15, 그담부터 +30
                    if (UpgradeNum[(int)upgradeSelect] == 0)
                    {
                        UpgradeButtonControl(upgradeSelect, DAMAGE_UP_COST, INIT_DAMAGE_PER_UPGRADE);
                    }
                    else
                        UpgradeButtonControl(upgradeSelect, DAMAGE_UP_COST, DAMAGE_PER_UPGRADE);
                    DAMAGE_UP_COST = DAMAGE_UP_COST + 10;
                }
                for (int idx = 0; idx < GunManager.Instance.gunList.Length; idx++)
                { // 총기 종류마다 데미지 싹 다 오름
                    GunManager.Instance.gunList[idx].damage += DAMAGE_PER_UPGRADE;
                }
                break;
            case UPGRADE_SELECT.RELOAD_SPEED:
                if (OnPlayScript.Instance.userDNA >= RELOAD_SPEED_UP_COST)
                {
                    UpgradeButtonControl(upgradeSelect, RELOAD_SPEED_UP_COST, RELOAD_SPEED_PER_UPGRADE);
                    RELOAD_SPEED_UP_COST = RELOAD_SPEED_UP_COST + 10;

                }
                for (int idx = 0; idx < GunManager.Instance.gunList.Length; idx++)
                {
                    GunManager.Instance.gunList[idx].reloadSpeed -= RELOAD_SPEED_PER_UPGRADE;
                }
                break;
            case UPGRADE_SELECT.ATTACK_SPEED:
                if (OnPlayScript.Instance.userDNA >= ATTACK_SPEED_UP_COST)
                {
                    UpgradeButtonControl(upgradeSelect, ATTACK_SPEED_UP_COST, ATTACK_SPEED_PER_UPGRADE);
                    ATTACK_SPEED_UP_COST = ATTACK_SPEED_UP_COST + 10;
                }

                for (int idx = 0; idx < GunManager.Instance.gunList.Length; idx++)
                {
                    GunManager.Instance.gunList[idx].attackSpeed -= ATTACK_SPEED_PER_UPGRADE;
                }
                break;
            case UPGRADE_SELECT.BUY_GRENADE:
                if (OnPlayScript.Instance.userDNA >= GRENADE_UP_COST)
                { // 수류탄은 1개씩 무한구매 가능.
                    OnPlayScript.Instance.userDNA -= GRENADE_UP_COST;
                    m_objGrenade.SetActive(true);
                    int count = GameManager.Instance.m_nGrenadeCount;
                    count = count + 1;
                    if (count > 0)
                    {
                        GameManager.Instance.m_objGrenadeOnBtn.SetActive(true);
                        GameManager.Instance.m_objGrenadeOffBtn.SetActive(false);
                    }
                    GameManager.Instance.m_nGrenadeCount = count;
                    TxtUpgradeList[(int)upgradeSelect].text =
                    count.ToString();

                    m_objGrenade.SetActive(false);

                }
                break;
            case UPGRADE_SELECT.MAX_UPGRADE_SELECT:
                break;
            default:
                Debug.Log(this + "Upgrade Err");
                break;
        }
        OnPlayScript.Instance.m_TxtUserDNA.text = OnPlayScript.Instance.userDNA.ToString();
    }

    void UpgradeButtonControl(UPGRADE_SELECT upgradeSelect, int needDna, int valuePerUpgrade)
    {
        OnPlayScript.Instance.userDNA -= needDna;
        UpgradeNum[(int)upgradeSelect]++;
        TxtUpgradeList[(int)upgradeSelect].text =
            "+" + (UpgradeNum[(int)upgradeSelect] * valuePerUpgrade).ToString();
        TxtUpgradeCost[(int)upgradeSelect].text = needDna.ToString();
        if (UpgradeNum[(int)upgradeSelect] == MAX_UPGRADE)
            m_BtnUpgradeList[(int)upgradeSelect].interactable = false;
    }
    void UpgradeButtonControl(UPGRADE_SELECT upgradeSelect, int needDna, float valuePerUpgrade)
    {
        OnPlayScript.Instance.userDNA -= needDna;
        UpgradeNum[(int)upgradeSelect]++;
        TxtUpgradeList[(int)upgradeSelect].text =
            (1.0f - (UpgradeNum[(int)upgradeSelect] * valuePerUpgrade)).ToString();

        if (UpgradeNum[(int)upgradeSelect] == MAX_UPGRADE)
            m_BtnUpgradeList[(int)upgradeSelect].interactable = false;
    }

}