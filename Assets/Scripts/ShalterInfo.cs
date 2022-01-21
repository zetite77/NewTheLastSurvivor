using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShalterInfo : MonoBehaviour
{
    public int m_nHp = 1000;
    public int m_nDef = 50;
    public int GetHp() { return m_nHp; }
    public void SetHp(int Hp) { m_nHp = Hp; }
    // Update is called once per frame
    void Update()
    {
        if (m_nHp <= 0)
            Destroy(this.gameObject);

    }
}
