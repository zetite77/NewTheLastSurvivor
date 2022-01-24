using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DnaPoint : MonoBehaviour
{
    public int m_nDnaPoint = 1;
    public float Timmer = 1.4f;
    public float m_fSpeed = 0.1f;
    public bool m_fGet = false;
    public Text m_DnaPointtext;
    GameManager gameManager;



 
    void Start()
    {
        int Rand = Random.Range(1, 16);
        m_nDnaPoint = Rand;
    }
    void Update()
    {
        // GetDnaPoint();
        if (GameManager.Instance.m_objTitleCanvas.activeSelf == true)
        {
            Destroy(this.gameObject);
        }
        Debug.Log("포인트획득");
        m_DnaPointtext.text = m_nDnaPoint.ToString();
        transform.position += Vector3.up * m_fSpeed * Time.deltaTime;
        Timmer = Timmer - Time.deltaTime;
        if (Timmer <= 0)
        {
            OnPlayScript.Instance.userDNA += m_nDnaPoint;
            OnPlayScript.Instance.m_TxtUserDNA.text = OnPlayScript.Instance.userDNA.ToString();
            Destroy(this.gameObject);
            
        }
    }
}
