using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Ranking : MonoBehaviour
{
    public GameObject m_Obj_LocalGlobal;
    public GameObject prefab_CvsUser;
    public GameObject m_Content;
    Canvas[] Cvs_user;

    struct RankObj
    {
        public string name;
        public string maxStage;
        public string maxZombieKills;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    { // 랭크 파일을 로드해서 배열에 격납한다 -> 점수순으로 정렬한다 -> 화면에 표시한다.
        RankObj[] rankArr;
        DeleteAllClone();
        rankArr = LoadRanking();
        quick_sort(rankArr, 0, rankArr.Length-1);
        CreateCvsClone(rankArr);
    }

    void DeleteAllClone()
    {
        if (Cvs_user != null)
        {
            for (int i = 0; i < Cvs_user.Length; i++)
            {
                Destroy(tempDestroy[i]);
            }
        }
    }

    enum STR_SPLIT { NAME_AREA, STAGE_AREA, KILLS_AREA, MAX_STR_SPLIT};
    RankObj[] LoadRanking()
    {
        string AllStr = File.ReadAllText(GameManager.Instance.localRankingPath);
        string[] splitRowStr = AllStr.Split('\n'); // 줄 단위로 나눠서 저장
        RankObj[] rankObj = new RankObj[splitRowStr.Length-1]; // 마지막 줄바꿈문자때문에 빈 인덱스 삭제

        for (int i = 0; i < rankObj.Length; i++)
        { // 각 줄마다 이름, 최대스테이지, 최대좀비킬을 나눠서 구조체에 저장.
            string[] splitColStr = new string[(int)STR_SPLIT.MAX_STR_SPLIT];
            splitColStr = splitRowStr[i].Split('$');
            rankObj[i].name = splitColStr[(int)STR_SPLIT.NAME_AREA];
            rankObj[i].maxStage = splitColStr[(int)STR_SPLIT.STAGE_AREA];
            rankObj[i].maxZombieKills = splitColStr[(int)STR_SPLIT.KILLS_AREA];
        }

        return rankObj;
    }

    GameObject[] tempDestroy = new GameObject[10000];
    void CreateCvsClone(RankObj[] rankArr)
    {
        Cvs_user = null;
        GameObject profile;
        GameObject name;
        GameObject Score;

        for (int i = 0; i < rankArr.Length; i++) // 랭킹 수만큼 캔버스 생성
            tempDestroy[i] = Instantiate<GameObject>(prefab_CvsUser, m_Content.transform);

        Cvs_user = m_Content.GetComponentsInChildren<Canvas>();
        for (int i = 0; i < rankArr.Length; i++)
        {
            profile = Cvs_user[i].transform.Find("Image").gameObject;
            name = Cvs_user[i].transform.Find("Name").gameObject;
            Score = Cvs_user[i].transform.Find("Score").gameObject;

            name.GetComponent<Text>().text = rankArr[i].name;
            Score.GetComponent<Text>().text = rankArr[i].maxStage + ", " + 
                rankArr[i].maxZombieKills;
        }
    }

    private static void quick_sort(RankObj[] arry, int left, int right)
    {
        if (left < right)
        {
            int PivotIndex = ArryDivide(arry, left, right);

            quick_sort(arry, left, PivotIndex - 1);
            quick_sort(arry, PivotIndex + 1, right);
        }
    }

    private static int ArryDivide(RankObj[] Arry, int left, int right)
    {
        RankObj temp;
        int PivotValue;
        int index_L, index_R;

        index_L = left;
        index_R = right;

        //Pivot 값은 0번 인덱스의 값을 가짐
        PivotValue = int.Parse(Arry[left].maxZombieKills);

        while (index_L < index_R)
        {
            //Pivot 값 보다 작은경우 index_L 증가(이동)
            while ((index_L <= right) && (int.Parse(Arry[index_L].maxZombieKills) < PivotValue))
                index_L++;
            //Pivot 값 보다 큰경우 index_R 감소(이동)
            while ((index_R >= left) && (int.Parse(Arry[index_R].maxZombieKills) > PivotValue))
                index_R--;
            //index_L 과 index_R 이 교차되지 않음
            if (index_L < index_R)
            {
                temp = Arry[index_L];
                Arry[index_L] = Arry[index_R];
                Arry[index_R] = temp;
                //같은 값이 존재 할 경우 
                if (int.Parse(Arry[index_L].maxZombieKills) == int.Parse(Arry[index_R].maxZombieKills))
                    index_R--;
            }
        }
        //index_L 과 index_R 이 교차된 경우 반복문을 나와 해당 인덱스값을 리턴
        return index_R;
    }
}
