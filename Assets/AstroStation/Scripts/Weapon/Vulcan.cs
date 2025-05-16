using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulcan : Weapon
{
    public override void Awake()
    {
        base.Awake();
        m_weaponLevel = 1f;
        m_damage = 1f;
    }

    private void Update()
    {
        if (!m_isFire)
            return;

        if(m_moveDistance >= m_maximumRange)
        {
            InitBullet();
        }

        float move = m_speed * timeManager.GetTimeScale();

        this.transform.Translate(Vector3.up * move);
        m_moveDistance += move;

        //
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
        float maxLevel = GameManager.instance.MainWeaponMaxLevel;
        if (m_weaponLevel >= maxLevel)
            return;

        m_weaponLevel++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        CharacterStatus stat = collision.GetComponent<CharacterStatus>();

        if (stat)
        {
            collision.GetComponent<CharacterStatus>().Damaged(m_damage);
            InitBullet();
            HitSFXPlay();
        }
    }
}
