using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : Weapon
{
    public GameObject m_smokeEff;
    public GameObject m_impactEff;

    private float m_initTime = 0;

    public override void Awake()
    {
        base.Awake();
        m_weaponLevel = 1f;
        m_damage = 5f;
    }

    void Update()
    {
        if (!m_isFire)
            return;

        if (m_moveDistance >= m_maximumRange)
        {
            m_initTime += timeManager.GetTimeScale();

            if (m_initTime > 3f)
            {
                InitBullet();
                m_initTime = 0f;
            }

            return;
        }

        float move = m_speed * timeManager.GetTimeScale();

        this.transform.Translate(Vector3.up * move);
        m_moveDistance += move;

        float bgBoundhalfSize = GameManager.instance.GetBgBoundSizeX() / 2f;

        if (this.transform.position.x <= GameManager.instance.spaceBg[0].position.x - bgBoundhalfSize)
        {
            float moveX = GameManager.instance.GetBgBoundSizeX() * 3f;
            this.transform.Translate(moveX, 0f, 0f);
        }
        else if (this.transform.position.x >= GameManager.instance.spaceBg[2].position.x + bgBoundhalfSize)
        {
            float moveX = GameManager.instance.GetBgBoundSizeX() * 3f;
            this.transform.Translate(-moveX, 0f, 0f);
        }
    }

    public override void Upgrade()
    {
        float maxLevel = GameManager.instance.SubWeaponMaxLevel;
        if (m_weaponLevel >= maxLevel)
            return;

        m_weaponLevel++;


        switch (m_weaponLevel)
        {
            case 2:
                m_damage = 10f;
                break;
            case 3:
                m_damage = 20f;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        CharacterStatus stat = collision.GetComponent<CharacterStatus>();

        if (stat)
        {
            collision.GetComponent<CharacterStatus>().Damaged(m_damage);

            if (!m_impactEff)
                return;

            GameObject eff = Instantiate(m_impactEff, this.transform.position, Quaternion.identity, null);
            Destroy(eff, 1.5f);
        }
    }
}
