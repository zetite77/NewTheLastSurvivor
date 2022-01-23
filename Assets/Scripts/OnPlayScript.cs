using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPlayScript : MonoBehaviour
{
    #region 싱글톤구현
    private static OnPlayScript _instance;
    public static OnPlayScript Instance
    {
        get
        {
            if (!_instance)
            {// 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                _instance = FindObjectOfType(typeof(OnPlayScript)) as OnPlayScript;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    #endregion

    public GameObject m_Shelter;
    public Image m_ShalterHPBar;
    public Image m_ShalterHpBarValue;
    public Image m_ProgBar_NightTime;
    public Text m_ProgBar_TestTxt;
    public Text m_Text_StageTxt;
    public Text m_TxtUserDNA;
    public int userDNA;


    // 스테이지 컨트롤
    private bool dayNightFlg = false; // 낮 = true, 밤 = false
    public float stageDuration; // 낮/밤 지속시간(단위:초)
    private int numberOfTime; // 몇 초 지났는지 int형으로 저장.
    public int numberOfStage = 1; // 스테이지 단계(항상 1단계부터 시작)
    const float PROGRESS_MAX = 1.0f; // 프로그래스 바 게이지 최대치(100%)
    const float PROGRESS_MIN = 0.0f;

    // 플레이 관련
    public int m_nGrenadecount = 3;

    private void Start()
    {
        m_ProgBar_NightTime.fillAmount = PROGRESS_MAX;
    }

    void Update()
    {
        if (dayNightFlg) DayProcess();
        else NightProcess();
    }

    void DayProcess()
    {
        if (m_ProgBar_NightTime.fillAmount < PROGRESS_MAX)
        { // 초당 1 / duration 만큼 부드럽게(매 프레임 마다) 증가 
            // duration이 15일 경우,         0초 일 때 1  ->  7.5초 일 때 0.5  ->  15초 일 때 0
            m_ProgBar_NightTime.fillAmount += PROGRESS_MAX / stageDuration * Time.deltaTime;

            #region 테스트用임. 나중에 지울것(변상현)
            // Math.Ceiling = double형 실수를 정수부분까지 올림
            numberOfTime = (int)System.Math.Ceiling(stageDuration - m_ProgBar_NightTime.fillAmount * stageDuration);
            m_ProgBar_TestTxt.text = numberOfTime + "s";
            #endregion
        }
        else
        { // 게이지가 모두 소진되면 밤 프로세스 시작
            InitNightProcess();
        }
    }

    public void ShalterHpBar()
    {
        if (m_ShalterHpBarValue.fillAmount > 0)
        {
            float Hp = GameManager.Instance.m_objShalter.GetComponent<ShalterInfo>().m_nHp;
            m_ShalterHpBarValue.fillAmount = Hp / 100;
        }
        else
        {
            StartCoroutine("GameOverCR");
            GameManager.Instance.GameOver("userName", numberOfStage, GunManager.Instance.zombieKills);
        }
    }
    IEnumerator GameOverCR()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(3.0f);
        GameManager.Instance.m_objOnPlayCanvas.SetActive(false);
        GameManager.Instance.m_objUpgradeCanvas.SetActive(false);
        GameManager.Instance.m_objTitleCanvas.SetActive(true);
    }

    public void InitStage()
    { // 스테이지 초기화 (게임을 아예 처음부터 시작하는 경우)
        numberOfStage = 0; // initNight하면서 1더함ㄱㅊ
        InitNightProcess();
        GameManager.Instance.m_objZombiResPoneRight.SetActive(true);
        GameManager.Instance.m_objZombiResPoneLeft.SetActive(true);
        GameManager.Instance.m_objShalter.SetActive(true);
        
    }

    public void InitNightProcess()
    {
        dayNightFlg = false;
        GameManager.Instance.m_objUpgradeCanvas.SetActive(false);
        m_ProgBar_NightTime.fillAmount = PROGRESS_MAX;

        m_ProgBar_TestTxt.text = stageDuration + "s"; // 테스트用임. 나중에 지울것(변상현)
        numberOfStage++;
        m_Text_StageTxt.text = "Stage" + numberOfStage;
        // 쉘터 체력 최대충전
        float Hp = GameManager.Instance.m_objShalter.GetComponent<ShalterInfo>().m_nHp;
        Hp = GameManager.Instance.m_objShalter.GetComponent<ShalterInfo>().m_MaxHp;
        ShalterHpBar();

        GameManager.Instance.ChangeMusic();
    }

    void NightProcess()
    {
        if (m_ProgBar_NightTime.fillAmount > PROGRESS_MIN)
        { // 초당 1 / duration 만큼 부드럽게(매 프레임 마다) 감소 
            // duration이 15일 경우,         0초 일 때 1  ->  7.5초 일 때 0.5  ->  15초 일 때 0
            m_ProgBar_NightTime.fillAmount -= PROGRESS_MAX / stageDuration * Time.deltaTime;

            #region 테스트用임. 나중에 지울것(변상현)
            // Math.Ceiling = double형 실수를 정수부분까지 올림
            numberOfTime = (int)System.Math.Ceiling(m_ProgBar_NightTime.fillAmount * stageDuration);
            m_ProgBar_TestTxt.text = numberOfTime + "s";
            #endregion
        }
        else
        { // 게이지가 모두 소진되면 낮 프로세스 시작
            GameManager.Instance.m_objUpgradeCanvas.SetActive(true);
            GameManager.Instance.m_objGrenade.GetComponent<Grenade>().SetCount(3);
            GameManager.Instance.m_objGrenadeOnBtn.SetActive(true);
            GameManager.Instance.m_objGrenadeOffBtn.SetActive(false);

            dayNightFlg = true;
            m_ProgBar_TestTxt.text = stageDuration + "s"; // 테스트用임. 나중에 지울것(변상현)
        }

        //OnPlayCanvas_EnableChanged();
        SetGranade();
    }

    public void SetGranade()
    {
        m_nGrenadecount = GameManager.Instance.m_objGrenade.GetComponent<Grenade>().GetCount();
        if (m_nGrenadecount <= 0)
        {
            GameManager.Instance.m_objGrenade.SetActive(false);
            GameManager.Instance.m_objGrenadeOnBtn.SetActive(false);
            GameManager.Instance.m_objGrenadeOffBtn.SetActive(true);
        }
    }
}
