using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour
{
    public int m_nHp = 300;

    public int GetHP () { return m_nHp; }
    public void SetHP (int Hp) { m_nHp = Hp; }
    // Update is called once per frame
    void Update()
    {
        if (m_nHp <= 0)
            Destroy(this.gameObject);
        
        
    }
}
