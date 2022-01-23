﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnaPoint : MonoBehaviour
{
    public int m_nDnaPoint = 1;
    GameManager gameManager;



    public void GetDnaPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 WorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(WorldPoint, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Dnapoint")
                {
                    DnaPoint dnaPoint = hit.transform.GetComponent<DnaPoint>();
                    Debug.Log("포인트획득");
                    OnPlayScript.Instance.userDNA += m_nDnaPoint;
                    OnPlayScript.Instance.m_TxtUserDNA.text = OnPlayScript.Instance.userDNA.ToString();
                    Destroy(this.gameObject);
                }
            }
        }
    }
    void Start()
    {
        int Rand = Random.Range(1, 16);
        m_nDnaPoint = Rand;
    }
    void Update()
    {
        GetDnaPoint();
        if (GameManager.Instance.m_objTitleCanvas.activeSelf == true)
        {
            Destroy(this.gameObject);
        }
    }
}
