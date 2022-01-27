using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions; // for ContinueWithOnMainThread

public class Ranking : MonoBehaviour
{
    public GameObject prefab_Cvs1st;
    public GameObject prefab_Cvs2nd;
    public GameObject prefab_Cvs3rd;
    public GameObject prefab_CvsUser;
    public GameObject m_CvsMyRanking;
    public GameObject m_Content;
    public List<GameObject> Cvs_user = new List<GameObject>();
    string strMyName = "K-ookbob";
    int myRanking;
    public int DISPLAY_CVS_NUM = 100; // 랭크화면에 올라갈 개체 개수


    int dbCount = 0; // db내용물 수
    bool dbCountCompleteFlg = false; // db숫자 파악 다했는지 확인하는 플래그
    bool dbReadCompleteFlg = false; // db를 다 읽었는지 확인하는 플래그

    struct RankObj
    {
        public string name;
        public int rank;
        public string maxStage;
        public string maxZombieKills;
    }

    private void Start()
    {
    }

    RankObj[] rankArr;
    int[] stageArr = new int[20];

    /// <summary>
    /// 코드 흐름 난해함 주의(async비동기 -> coroutine동기)
    /// 요약 : 랭크를 firebase에서 받아서 배열에 격납한다 -> 점수순으로 정렬한다 -> 화면에 표시한다.
    /// </summary>
    private void OnEnable()
    {  
        // 초기화
        StopAllCoroutines(); 
         dbCount = 0;
         dbCountCompleteFlg = false;
         dbReadCompleteFlg = false;

        // 랭킹 알고리즘
        StartCoroutine(LoadingCanvas());
        AsyncDataCount();
        StartCoroutine(UntilDataCount());
    }

    private void OnDisable()
    {
        for (int i = 0; i < Cvs_user.Count; i++)
            Destroy(Cvs_user[i]);
        Cvs_user.Clear();

        GameManager.Instance.m_objInLoadingCanvas.SetActive(false);
    }
    
    IEnumerator LoadingCanvas()
    {
        GameManager.Instance.m_objInLoadingCanvas.SetActive(true);
        GameManager.Instance.m_objInLoadingCanvas.GetComponentInChildren<Text>().text = "Loading...";
        yield return new WaitForSeconds(5.0f);
        GameManager.Instance.m_objInLoadingCanvas.GetComponentInChildren<Text>().text = "Loading Failed";
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.m_objInLoadingCanvas.SetActive(false);
    }

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

    IEnumerator UntilDataCount()
    {
        while (!dbCountCompleteFlg)
        {
            Debug.Log("DataCountWating...");
            yield return new WaitForSeconds(.5f);
        }

        Debug.Log("Read dbCount complete : "+dbCount);
        rankArr = new RankObj[dbCount];
        AsyncDataRead();
        StartCoroutine(UntilDbRead());
    }

    void AsyncDataRead()
    {
        for (int i = 0; i < dbCount; i++)
        {
            FirebaseDatabase.DefaultInstance
            .GetReference(i.ToString())
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("ServerReadFailed : " + task.ToString());
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    rankArr[int.Parse(snapshot.Key)].name = snapshot.Child("name").Value.ToString();
                    rankArr[int.Parse(snapshot.Key)].maxStage = snapshot.Child("maxStage").Value.ToString();
                    rankArr[int.Parse(snapshot.Key)].maxZombieKills = snapshot.Child("maxZombieKills").Value.ToString();

                    if (snapshot.Key == (dbCount - 1).ToString())
                        dbReadCompleteFlg = true;
                }
            });
        }
    }

    IEnumerator UntilDbRead()
    {
        while (!dbReadCompleteFlg)
        {
            Debug.Log("DataReadWating...");
            yield return new WaitForSeconds(.5f);
        }

        QuickSort(rankArr, 0, rankArr.Length - 1, SORTMODE.STAGE);

        int preMaxStage = 20;
        int sortStartLeft = 0;
        for (int i = 0; i < rankArr.Length; i++)
        {
            if (int.Parse(rankArr[i].maxStage) != preMaxStage)
            { // maxStage가 변한경우
                QuickSort(rankArr, sortStartLeft, i - 1, SORTMODE.KILL) ;
                sortStartLeft = i;
            }
            else if (i == rankArr.Length - 1)
            {
                QuickSort(rankArr, sortStartLeft, i, SORTMODE.KILL);
            }
            preMaxStage = int.Parse(rankArr[i].maxStage);
        }

        for (int i = 0; i < rankArr.Length; i++) // 랭크(숫자)설정
            rankArr[i].rank = i + 1;

        SetMyRanking();
        SetRanking(rankArr);

        dbCountCompleteFlg = false;
        dbReadCompleteFlg = false;

        StopCoroutine(LoadingCanvas());
        GameManager.Instance.m_objInLoadingCanvas.SetActive(false);
    }

    void SetMyRanking()
    {
        RankObj myMaxRanking = new RankObj();
        myMaxRanking.rank = 10000;
        foreach (var item in rankArr)
        {
            if (item.name == strMyName && item.rank < myMaxRanking.rank)
            { // 내 최대 랭킹만 갱신 & 출력
                m_CvsMyRanking.transform.Find("Name").GetComponent<Text>().text = strMyName;
                m_CvsMyRanking.transform.Find("Score").GetComponent<Text>().text =
                    item.maxStage.ToString()+ ", " +  item.maxZombieKills.ToString();
                m_CvsMyRanking.transform.Find("Txt_Rank").GetComponent<Text>().text = 
                    "#" + item.rank.ToString();
                myRanking = item.rank;
                myMaxRanking = item;
            }
        }
    }

    void SetRanking(RankObj[] rankArr)
    {
        GameObject name;
        GameObject score;
        GameObject txtRank;
        int displayStartNum = (myRanking - 1) / DISPLAY_CVS_NUM;

        //1~3순위 생성
        Cvs_user.Add(Instantiate<GameObject>(prefab_Cvs1st, m_Content.transform));
        Cvs_user.Add(Instantiate<GameObject>(prefab_Cvs2nd, m_Content.transform));
        Cvs_user.Add(Instantiate<GameObject>(prefab_Cvs3rd, m_Content.transform));

        for (int i = 0; 
            i < DISPLAY_CVS_NUM && // i가 100보다 작다면
            displayStartNum * DISPLAY_CVS_NUM + i < rankArr.Length;  // 랭커숫자가 부족해서 100명뽑는데 중간에 짤린다면
            i++)
        { // 초기화 리스트 +100 : 최대 총 103개
            Cvs_user.Add(Instantiate<GameObject>(prefab_CvsUser, m_Content.transform));
        }

        for (int i = 0; i < 3; i++)
        { // 순위권 내 랭킹
            name = Cvs_user[i].transform.Find("Name").gameObject;
            score = Cvs_user[i].transform.Find("Score").gameObject;
            name.GetComponent<Text>().text = rankArr[i].name;
            score.GetComponent<Text>().text = rankArr[i].maxStage + ", " +
                rankArr[i].maxZombieKills;
        }

        for (int i = 3; i < Cvs_user.Count; i++)
        { // 순위권 외 랭킹
            name = Cvs_user[i].transform.Find("Name").gameObject;
            score = Cvs_user[i].transform.Find("Score").gameObject; 
            txtRank = Cvs_user[i].transform.Find("Txt_Rank").gameObject;

            name.GetComponent<Text>().text = 
                rankArr[i-3 + displayStartNum * DISPLAY_CVS_NUM].name;
            score.GetComponent<Text>().text = 
                rankArr[i-3 + displayStartNum * DISPLAY_CVS_NUM].maxStage + ", " +
                rankArr[i-3 + displayStartNum * DISPLAY_CVS_NUM].maxZombieKills;
            txtRank.GetComponent<Text>().text = 
                "#" + rankArr[i-3 + displayStartNum * DISPLAY_CVS_NUM].rank.ToString();
        }
    }

    enum SORTMODE { STAGE, KILL }
    private void QuickSort(RankObj[] arry, int left, int right, SORTMODE mode)
    {
        if (left >= right)
            return;

        int pivot = left;
        int i = pivot + 1;
        int j = right;
        RankObj temp;

        while (i <= j)
        {
            switch (mode)
            {
                case SORTMODE.STAGE:
                    while (i <= right && int.Parse(arry[i].maxStage) >= int.Parse(arry[pivot].maxStage))
                        i++;
                    while (j > left && int.Parse(arry[pivot].maxStage) >= int.Parse(arry[j].maxStage))
                        j--;
                    break;

                case SORTMODE.KILL:
                    while (i <= right && int.Parse(arry[i].maxZombieKills) >= int.Parse(arry[pivot].maxZombieKills))
                        i++;
                    while (j > left && int.Parse(arry[pivot].maxZombieKills) >= int.Parse(arry[j].maxZombieKills))
                        j--;
                    break;

                default:
                    break;
            }

            if (i > j)
            { // 엇갈림
                temp = arry[j];
                arry[j] = arry[pivot];
                arry[pivot] = temp;
            }
            else
            {// i와 j를 스왑
                temp = arry[i];
                arry[i] = arry[j];
                arry[j] = temp;
            }
        }
        QuickSort(arry, left, j - 1, mode);
        QuickSort(arry, j + 1, right, mode);
    }
}
