using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    #region 싱글톤구현
    private static GunManager _instance;
    public static GunManager Instance
    {
        get
        {
            if (!_instance)
            {// 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                _instance = FindObjectOfType(typeof(GunManager)) as GunManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    #endregion

    public Text m_TxtRemainBullet;
    public Image m_GunImage;
    public Sprite[] m_GusSprite = new Sprite[MAX_OF_GUN_KIND];
    public AudioSource m_ShotSound01;
    public AudioSource m_ReloadSound01;

    public struct Gun
    {
        public int gunLevel; // 총기종류
        public int damage;
        public float attackSpeed;
        public float reloadSpeed;
        public int BulletMaxSize; // 최대 탄창수

        public AudioSource shotSound; // 각 총기별 발사음
    }// 수류탄은 0번

    public int remainBullet; // 남은 탄약
    const int MAX_OF_GUN_KIND = 11; // 총기 10종 + 수류탄 1
    public int DMG_PER_GUN_LEVEL = 5;
    public Gun[] gunList = new Gun[MAX_OF_GUN_KIND];
    public int currentGunPtr = 1; // 현재 사용 할 총기의 인덱스
    public int zombieKills = 0;

    // 수치 모니터링 test용. 제출시 삭제
    public int MNT_WEAPON_DMG;
    public float MNT_ATTACK_SPEED;
    public float MNT_RELOAD_SPEED;

    public void StatMonitoring()
    { // 유니티 안에서 수치를 바꾸면 현재총기에 즉시적용
        gunList[currentGunPtr].damage = MNT_WEAPON_DMG;
        gunList[currentGunPtr].attackSpeed = MNT_ATTACK_SPEED;
        gunList[currentGunPtr].reloadSpeed = MNT_RELOAD_SPEED;
    }
    /////////////////////////////////////////////////////////////////////////
 
    void Start()
    {
        for (int idx=0;  idx < MAX_OF_GUN_KIND; idx++)
        {
            gunList[idx].gunLevel = idx;
            gunList[idx].damage = idx * DMG_PER_GUN_LEVEL + 5;
            gunList[idx].attackSpeed = 1.0f;
            gunList[idx].reloadSpeed = 1.0f;
            gunList[idx].shotSound = m_ShotSound01;
        }
        gunList[0].damage = 300;
        gunList[0].BulletMaxSize = 3;
        gunList[1].BulletMaxSize = 7;
        gunList[2].BulletMaxSize = 25;
        gunList[3].BulletMaxSize = 30;
        gunList[4].BulletMaxSize = 30;
        gunList[5].BulletMaxSize = 20;
        gunList[6].BulletMaxSize = 5;
        gunList[7].BulletMaxSize = 100;
        gunList[8].BulletMaxSize = 120;
        gunList[9].BulletMaxSize = 1;
        gunList[10].BulletMaxSize = 6;

        remainBullet = gunList[1].BulletMaxSize;
        m_TxtRemainBullet.text = remainBullet.ToString();


        // 수치 모니터링용. 제출시 삭제
        MNT_WEAPON_DMG = gunList[currentGunPtr].damage;
        MNT_ATTACK_SPEED = gunList[currentGunPtr].attackSpeed;
        MNT_RELOAD_SPEED = gunList[currentGunPtr].reloadSpeed;
        /////////////////////////////////////////////////////////////////////////

    }
    private void OnEnable()
    {
        // 게임 중 setup으로 게임을 종료exit하면 GunManager.cs가 비활성화돼서 코루틴이 선채로죽음.
        // active 될 때마다 한번 눕혀줘야겠음.
        StopCoroutine("GunShot");
        gunShotIsRunning = false;
    }

    void Update()
    {
        if (GameManager.Instance.m_objSetupCanvas.activeSelf || 
            GameManager.Instance.m_objUpgradeCanvas.activeSelf)
        { //  셋팅이나 업그레이드 캔버스가 켜져 있을 경우, 발사금지
        }
        else
        {
            switch (gunList[currentGunPtr].gunLevel)
            {
                case 1:case 5:case 6:case 9:case 10: // 단발형 총기
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (remainBullet > 0)
                        { // 발사(터치)
                            if (!gunShotIsRunning)
                                StartCoroutine("GunShot");
                        }
                        else
                        {
                            if (!gunShotIsRunning) {
                                StopCoroutine("GunReload");
                                StartCoroutine("GunReload");
                            }
                        }
                    }
                    break;
                case 2:case 3:case 4:case 7:case 8: // 연발형 총기
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        if (remainBullet > 0)
                        {
                            if (!gunShotIsRunning)
                                StartCoroutine("GunShot");
                        }
                        else
                        {
                            if (!gunShotIsRunning)
                            {
                                StopCoroutine("GunReload");
                                StartCoroutine("GunReload");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

        }

        StatMonitoring(); // test용. 제출시 삭제할 것
        GunLevelup(); // test용. 제출시 삭제할 것
    }

    bool gunShotIsRunning = false;

    IEnumerator GunShot()
    {
        gunShotIsRunning = true; // 총기 공속 딜레이 중에는 다음 탄 발사 금지

        gunList[currentGunPtr].shotSound.Play();
        remainBullet--;
        m_TxtRemainBullet.text = remainBullet.ToString();
        yield return new WaitForSeconds(gunList[currentGunPtr].attackSpeed);

        gunShotIsRunning = false;
    }

    IEnumerator GunReload()
    {
        gunShotIsRunning = true;

        m_ReloadSound01.Play();
        m_TxtRemainBullet.text = remainBullet.ToString(); // 0
        yield return new WaitForSeconds(gunList[currentGunPtr].reloadSpeed);
        remainBullet = gunList[currentGunPtr].BulletMaxSize;
        m_TxtRemainBullet.text = remainBullet.ToString(); // 탄약최대치

        gunShotIsRunning = false;
    }

    void GunLevelup()
    {
        m_GunImage.sprite = m_GusSprite[currentGunPtr];
    }
}
