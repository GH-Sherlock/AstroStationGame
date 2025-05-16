using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WearAbleManager : Singleton<WearAbleManager>
{
    private MainWeaponRequest mainWeapon;
    private SubWeaponRequest subWeapon;

    private void Awake()
    {
        Debug.Log("WearAbleManager Awake");
        mainWeapon = this.gameObject.AddComponent<MainWeaponRequest>();
        subWeapon = this.gameObject.AddComponent<SubWeaponRequest>();
    }

    public void GoogleURL()
    {
        Application.OpenURL("http://google.com");
    }

    public void MainWeaponNotFire()
    {
        mainWeapon.MainWeaponWait();
    }

    public void VulcanFire()
    {
        mainWeapon.Vulcan();
    }

    public void LaserBeamFire()
    {
        mainWeapon.LaserBeam();
    }

    public void SubWeaponNotFire()
    {
        subWeapon.SubWeaponNotWait();
    }

    public void RailGunFire()
    {
        subWeapon.RailGun();
    }

    public void HomingMissileFire()
    {
        subWeapon.HomingMissile();
    }

    public void DragonBreathFire()
    {
        subWeapon.DragonBreath();
    }

}
