using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponItem : ItemBase
{
    private MainWeaponType m_mainWeaponType;

    public override void WeaponTypeUpdate()
    {
        switch (m_enableNumber)
        {
            case 0:
                m_mainWeaponType = MainWeaponType.Vulcan;
                break;
            case 1:
                m_mainWeaponType = MainWeaponType.Beam;
                break;
            case 2:
                m_mainWeaponType = MainWeaponType.Plasma;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_getSFX)
                SoundController.instance.PlaySFX(m_getSFX);

            PlayerShipController p = collision.GetComponent<PlayerShipController>();

            p.MainWeaponUpgrade(m_mainWeaponType);

            Destroy(this.gameObject);
        }
    }
}
