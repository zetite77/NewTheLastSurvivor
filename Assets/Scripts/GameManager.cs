using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region 싱글톤구현
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {// 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    #endregion

    public GameObject m_objOnPlayCanvas; //수정(김상현)22.01.17 원코드 : private Canvas = Cvs_OnPlayCanvas;
    public GameObject m_objTitleCanvas;
    public GameObject m_objUpgradeCanvas;
    public GameObject m_objInGamePopupCanvas;
    public GameObject m_objSetupCanvas;
    public GameObject m_objGrenade;
    public GameObject m_objGrenadeOnBtn;
    public GameObject m_objGrenadeOffBtn;
    public GameObject m_SoundEffectParents;
    public GameObject m_objZombiResPoneLeft;
    public GameObject m_objZombiResPoneRight;
    public GameObject m_objZombiResPoneBot;
    public GameObject m_objZombiResPoneTop;
    public GameObject m_objShalter;

    // 오디오변수
    public AudioSource[] m_NightSound; // 스테이지 시작 사운드 트랙
    public AudioSource[] m_SoundEffect;
    public AudioSource[] m_BackgroundMusic; // 배경음, [0] : Intro, [1] : Ingame
    int soundTrackNum = 0;
    int preTrackNum = 0;
    public bool vibration = true;

    // 랭킹 변수
    public string rankingDirectory = "Ranking/";
    public string localRankingPath = "Ranking/LocalRanking.txt";
    // 스맛폰으로 플레이 시 확인이 안됨. 수정바람

    public int m_nGrenadeCount;

    public void Grenade()
    {
        if (m_objGrenade.activeSelf == true)
        {
            m_nGrenadeCount = m_objGrenade.GetComponent<Grenade>().m_nCount;
        }
        else
        {
            m_objGrenade.GetComponent<Grenade>().m_nCount = m_nGrenadeCount;
        }
    }

    void Start()
    {
        if (m_objGrenade.activeSelf == true)
        {
            if (m_objOnPlayCanvas.GetComponent<OnPlayScript>().numberOfStage == 1)
            {
                m_nGrenadeCount = 3;

            }
        }
            m_SoundEffect = m_SoundEffectParents.GetComponentsInChildren<AudioSource>();

        // 초기 볼륨 50%설정
        const float INIT_VOLUME = 0.5f;
        foreach (AudioSource soundEffect in m_SoundEffect)
            soundEffect.volume = INIT_VOLUME;
        m_BackgroundMusic[0].volume = INIT_VOLUME;
        m_BackgroundMusic[1].volume = INIT_VOLUME;
        m_BackgroundMusic[0].Play();

        // 랭킹 파일 생성 (Ranking/LocalRanking.txt)
        if (!Directory.Exists(rankingDirectory))
            Directory.CreateDirectory(rankingDirectory);
        if (!File.Exists(localRankingPath))
            File.Create(localRankingPath);
    }

    void Update()
    {
        if (m_objTitleCanvas.activeSelf == true)
            m_nGrenadeCount = 3;
        Grenade();
        if(m_nGrenadeCount >= 1)
        {
            if (this.gameObject.activeSelf == false)
            {
                m_objGrenadeOnBtn.SetActive(true);
                m_objGrenadeOffBtn.SetActive(false);
            }
            m_objGrenade.GetComponent<Grenade>().m_GrenadeOnBtnText.text = m_nGrenadeCount.ToString();
        }
        m_objOnPlayCanvas.GetComponent<OnPlayScript>().SetGranade();
    }
 

    public void ChangeMusic()
    { // 스테이지 시작 효과음 재생 및 변경
        m_NightSound[soundTrackNum].Play();
        preTrackNum = soundTrackNum;
        soundTrackNum = (soundTrackNum < m_NightSound.Length - 1) ? (soundTrackNum + 1) : 0;
    }

    public void RankUpload(string userName, int stage, int zombieKills)
    {
        // 플레이 점수 로컬에 저장
        FileStream fileStream = new FileStream(localRankingPath, FileMode.Append);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);
        writer.WriteLine(userName + "$" + stage + "$" + zombieKills);
        writer.Close();

    }

    public IEnumerator InGamePopup(string str)
    {
        m_objInGamePopupCanvas.SetActive(true);
        m_objInGamePopupCanvas.GetComponentInChildren<Text>().text = str;
        yield return new WaitForSeconds(1.0f);
        m_objInGamePopupCanvas.SetActive(false);
    }

    public void GameClear()
    {
        StartCoroutine(InGamePopup("Clear!!!"));
        RankUpload("ClearUser", OnPlayScript.Instance.numberOfStage, GunManager.Instance.zombieKills);
        System.Threading.Thread.Sleep(1001);
        m_objOnPlayCanvas.SetActive(false);
        m_objTitleCanvas.SetActive(true);
    }

}
