using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShalterInfo : MonoBehaviour
{
    public int m_nHp = 100;
    public int m_nInitHp = 100;
    public int m_nDef = 5;
    public int GetHp() { return m_nHp; }
    public void SetHp(int Hp) { m_nHp = Hp; }
    // Update is called once per frame
    void Update()
    {
        if (m_nHp <= 0)
            //Destroy(this.gameObject); 
            this.gameObject.SetActive(false); // destroy하면 다음 게임 시작했을때 죽은상태 그대로

    }
    
    
}
