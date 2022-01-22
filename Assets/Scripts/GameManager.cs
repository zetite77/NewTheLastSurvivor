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

    //public GameObject m_objOnPlayCanvas; //수정(김상현)22.01.17 원코드 : private Canvas = Cvs_OnPlayCanvas;
    public GameObject m_objTitleCanvas;
    public GameObject m_objUpgradeCanvas;
    public GameObject m_objSetupCanvas;
    public GameObject m_objGrenade;
    public GameObject m_objGrenadeOnBtn;
    public GameObject m_objGrenadeOffBtn;
    public GameObject m_SoundEffectParents;
    public GameObject m_objZombiResPoneLeft;
    public GameObject m_objZombiResPoneRight;
    public GameObject m_objShalter;

    // 오디오
    public Button Btn_Next;
    public Button GameStartButton;
    public Button GameStartButton2; // 랭크에서도 게임스타트 가능함
    public AudioSource[] m_NightSound; // 스테이지 시작 사운드 트랙
    public AudioSource[] m_SoundEffect;
    public AudioSource m_BackgroundMusic; // 배경음
    int soundTrackNum = 0;
    int preTrackNum = 0;


    void Start()
    {
        m_SoundEffect = m_SoundEffectParents.GetComponentsInChildren<AudioSource>();

        // 초기 볼륨 50%설정
        const float INIT_VOLUME = 0.5f;
        foreach (AudioSource soundEffect in m_SoundEffect)
            soundEffect.volume = INIT_VOLUME;
        m_BackgroundMusic.volume = INIT_VOLUME;


        if (!Directory.Exists(rankingDirectory))
            Directory.CreateDirectory(rankingDirectory);
        if (!File.Exists(localRankingPath))
            File.Create(localRankingPath);
    }

    void Update()
    {
        if(m_objTitleCanvas.activeSelf == true)
        {
            m_objShalter.SetActive(false);
        }
    }

    public void ChangeMusic()
    { // 스테이지 시작 효과음 재생 및 변경
        m_NightSound[soundTrackNum].Play();
        preTrackNum = soundTrackNum;
        soundTrackNum = (soundTrackNum < m_NightSound.Length - 1) ? (soundTrackNum + 1) : 0;
    }

    string rankingDirectory = "Ranking/";
    string localRankingPath = "Ranking/LocalRanking.txt";
    public void GameOver(string userName, int stage, int zombieKills)
    {
        // 플레이 점수 로컬에 저장
        FileStream fileStream = new FileStream(localRankingPath, FileMode.Append);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);
        writer.WriteLine("#" + userName + "%" + stage + "$" + zombieKills);
        writer.Close();

    }

}
