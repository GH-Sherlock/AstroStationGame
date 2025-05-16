using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : ItemBase
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

            CharacterStatus p = collision.GetComponent<CharacterStatus>();
            p.HpPlus(1f);

            Destroy(this.gameObject);
        }
    }
}
