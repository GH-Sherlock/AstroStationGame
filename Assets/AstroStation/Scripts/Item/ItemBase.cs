using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public int typeNumber;
    public GameObject[] m_item;
    public float m_toggleTime = 3f;

    protected float m_elapsedTime = 0f;
    protected int m_enableNumber = 0;

    public AudioClip m_getSFX;

    virtual public void Awake()
    {
        //0~3
        m_enableNumber = Random.Range(0, typeNumber);


        for (int i = 0; i < typeNumber; i++)
        {
            m_item[i].SetActive(false);
        }
        m_item[m_enableNumber].SetActive(true);
        WeaponTypeUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        //단일 아이템이면 업데이트 하지 않음
        if (typeNumber == 0)
            return;

        m_elapsedTime += TimeManager.instance.GetTimeScale();

        if (m_toggleTime <= m_elapsedTime)
        {
            m_elapsedTime = 0f;

            m_item[m_enableNumber].SetActive(false);
            m_enableNumber++;
            m_enableNumber = m_enableNumber % typeNumber;
            m_item[m_enableNumber].SetActive(true);
            WeaponTypeUpdate();
        }
    }

    virtual public void WeaponTypeUpdate() { }
}
