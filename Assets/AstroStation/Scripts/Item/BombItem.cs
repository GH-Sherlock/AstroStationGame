using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : ItemBase
{
    public override void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_getSFX)
                SoundController.instance.PlaySFX(m_getSFX);

            Bomb b = collision.GetComponent<PlayerShipController>().m_bomb;
            b.UsageCount = b.UsageCount + 1;

            Destroy(this.gameObject);
        }
    }
}
