using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponItem : ItemBase
{
    private SubWeaponType m_subWeaponType;

    public override void Awake()
    {
        base.Awake();
    }

    public override void WeaponTypeUpdate()
    {
        switch (m_enableNumber)
        {
            case 0:
                m_subWeaponType = SubWeaponType.RailGun;
                break;
            case 1:
                m_subWeaponType = SubWeaponType.DragonBreath;
                break;
            case 2:
                m_subWeaponType = SubWeaponType.HomingMissile;
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

            p.SubWeaponUpgrade(m_subWeaponType);

            Destroy(this.gameObject);
        }
    }
}
