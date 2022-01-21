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
    public AudioSource m_ShotSound01;
    public AudioSource m_ReloadSound01;

    public struct Gun
    {
        public int level; // 총기종류
        public int damage;
        public float attackSpeed;
        public float reloadSpeed;
        public int BulletMaxSize; // 최대 탄창수

        public AudioSource shotSound; // 각 총기별 발사음
    }// 수류탄은 0번

    public int remainBullet; // 남은 탄약
    const int MAX_OF_GUN_KIND = 11; // 총기 10종 + 수류탄 1
    public int DMG_PER_LEVEL = 5;
    public float ATTACK_SPEED_PER_LEVEL = 0.02f;
    public float RELOAD_SPEED_PER_LEVEL= 0.02f;
    public Gun[] gunList = new Gun[MAX_OF_GUN_KIND];
    public int currentGunPtr = 1; // 현재 사용 할 총기의 인덱스

    // 수치 모니터링 test용. 제출시 삭제할 것
    public int MNT_WEAPON_DMG;
    public float MNT_ATTACK_SPEED;
    public float MNT_RELOAD_SPEED;

    public void StatMonitoring()
    {
        MNT_WEAPON_DMG = gunList[currentGunPtr].damage;
        MNT_ATTACK_SPEED = gunList[currentGunPtr].attackSpeed;
        MNT_RELOAD_SPEED = gunList[currentGunPtr].reloadSpeed;
    }
    /////////////////////////////////////////////////////////////////////////
 
    void Start()
    {
        for (int idx=0;  idx < MAX_OF_GUN_KIND; idx++)
        {
            gunList[idx].level = idx;
            gunList[idx].damage = idx * DMG_PER_LEVEL + 5;
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

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (remainBullet> 0)
            { // 발사(터치)
                gunList[currentGunPtr].shotSound.Play();
                remainBullet--;
                m_TxtRemainBullet.text = remainBullet.ToString();
            }
            else
            {
                StopCoroutine("GunReload");
                StartCoroutine("GunReload");
            }
        }

        StatMonitoring(); // test용. 제출시 삭제할 것
    }

    IEnumerator GunReload()
    {
        m_ReloadSound01.Play();
        m_TxtRemainBullet.text = remainBullet.ToString(); // 0
        yield return new WaitForSeconds(MNT_RELOAD_SPEED);
        remainBullet = gunList[currentGunPtr].BulletMaxSize;
        m_TxtRemainBullet.text = remainBullet.ToString(); // 탄약최대치
    }


}
