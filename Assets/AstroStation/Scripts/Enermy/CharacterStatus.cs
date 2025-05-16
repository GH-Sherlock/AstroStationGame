using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public float hp;
    public float hpMax;
    private bool m_isLockOn;

    public HpUI playerHpUI;

    public void Damaged(float dmg)
    {
        hp -= dmg;

        if (hp < 0)
            hp = 0;

        if (playerHpUI)
            playerHpUI.HpIconUpdate(hp);
    }

    public void HpPlus(float value)
    {
        hp += value;

        if(hp > hpMax)
        {
            hp = 3;
        }

        if (playerHpUI)
            playerHpUI.HpIconUpdate(hp);
    }

    //유도 미사일 관련
    public bool IsLockOn
    {
        get { return m_isLockOn; }
        set { m_isLockOn = value; }
    }
}
