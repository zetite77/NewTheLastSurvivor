using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

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
    struct UploadRankObj
    {
        public string name;
        public string maxStage;
        public string maxZombieKills;
    }

    public GameObject m_objOnPlayCanvas; //수정(김상현)22.01.17 원코드 : private Canvas = Cvs_OnPlayCanvas;
    public GameObject m_objTitleCanvas;
    public GameObject m_objUpgradeCanvas;
    public GameObject m_objInGamePopupCanvas;
    public GameObject m_objSetupCanvas;
    public GameObject m_objGrenade;
    public GameObject m_objGrenadeOnBtn;
    public GameObject m_objGrenadeOffBtn;
    public GameObject m_SoundEffectParents;
    public GameObject m_objShalter;
    public GameObject m_objGrenadeEffect;

    public GameObject m_objZombiResPoneTop;
    public GameObject m_objZombiResPoneLeft;
    public GameObject m_objZombiResPoneLeft2;
    public GameObject m_objZombiResPoneRight;
    public GameObject m_objZombiResPoneRight2;
    public GameObject m_objZombiResPoneBot;
    public GameObject m_objZombiResPoneBot2;

    // 오디오변수
    public AudioSource[] m_NightSound; // 스테이지 시작 사운드 트랙
    public AudioSource[] m_SoundEffect;
    public AudioSource[] m_BackgroundMusic; // 배경음, [0] : Intro, [1] : Ingame
    int soundTrackNum = 0;
    int preTrackNum = 0;
    public bool vibration = true;

    public int m_nGrenadeCount;
    public float m_fTime = 0.6f;
    public void Grenade()
    {
        if (m_objGrenade.GetComponent<Grenade>().m_bfire == true)
        {
            Vector2 mouseposition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 objposition = Camera.main.ScreenToWorldPoint(mouseposition);
            Vector2 curposition = new Vector2(m_objGrenadeEffect.transform.position.x, m_objGrenadeEffect.transform.position.y);
            if(m_objGrenadeEffect.activeSelf == false)
            { 
                m_objGrenadeEffect.transform.position = objposition;
            }
            else
            m_objGrenadeEffect.transform.position = curposition;
            m_objGrenadeEffect.SetActive(true);
            m_fTime = m_fTime - Time.deltaTime;
            if (m_fTime <= 0)
            {
                m_objGrenade.GetComponent<Grenade>().m_bfire = false;
                m_objGrenadeEffect.SetActive(false);
                m_fTime = 0.7f;
            }
        }
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

    public void RankUpload(string _userName, int _stage, int _zombieKills)
    {   // 플레이 점수 파이어베이스에 저장
        AsyncDataCount();
        StartCoroutine(UntilDataCount(_userName, _stage, _zombieKills));

    }
    int dbCount = 0;
    bool dbCountCompleteFlg = false;
    void AsyncDataCount()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;
            dbCount = (int)snapshot.ChildrenCount;
            dbCountCompleteFlg = true;
        });
    }

    IEnumerator UntilDataCount(string _userName, int _stage, int _zombieKills)
    {
        while (!dbCountCompleteFlg)
        {
            Debug.Log("DataCountWating...");
            yield return new WaitForSeconds(.5f);
        }

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        UploadRankObj user = new UploadRankObj();
        user.name = _userName;
        user.maxStage = _stage.ToString();
        user.maxZombieKills = _zombieKills.ToString();
        string json = JsonUtility.ToJson(user);

        reference.Child((dbCount).ToString()).SetRawJsonValueAsync(json);
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
        RankUpload("K-ookbob", OnPlayScript.Instance.numberOfStage, GunManager.Instance.zombieKills);
        System.Threading.Thread.Sleep(1001);
        m_objOnPlayCanvas.SetActive(false);
        m_objTitleCanvas.SetActive(true);
    }

}
