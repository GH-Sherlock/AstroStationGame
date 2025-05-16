using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponPool : MonoBehaviour
{
    public Transform m_firePos;
    private TimeManager timeManager;
    private float m_elapsedTime;


    //호밍미사일
    public int m_homingPoolSize;
    public GameObject m_pHomingMissile;
    public Transform m_homingMissilePool;
    private List<HomingMissile> m_homingMissileList;
    [SerializeField] private int m_homingMissileLevel;
    private Quaternion m_homingMissileDir;
    [SerializeField] private AudioClip m_homingMissileSFX;

    //레일건
    public int m_railGunPoolSize;
    public GameObject m_pRailgun;
    public Transform m_railGunPool;
    private List<Railgun> m_railGunList;
    [SerializeField] private int m_railGunLevel;
    private Quaternion m_railGunDir;
    [SerializeField] private AudioClip m_railGunSFX;


    //드래곤 브레스
    public GameObject m_pDragonBreath;
    public Transform m_dragonBreathPool;
    private DragonBreath m_dragonBreath;
    [SerializeField] private int m_dragonBreathLevel;
    [SerializeField] private float m_dragonBreathDurationTime;
    [SerializeField] private AudioClip m_dragonBreathSFX;


    private void Awake()
    {
        timeManager = TimeManager.instance;
        InitWeapons();

    }

    private void InitWeapons()
    {
        //호밍미사일
        m_homingMissileDir = Quaternion.identity;
        m_homingMissileLevel = 1;
        m_homingMissileList = new List<HomingMissile>();

        for(int i = 0; i < m_homingPoolSize; i++)
        {
            GameObject homingMissile = Instantiate(m_pHomingMissile, m_homingMissilePool);
            homingMissile.SetActive(false);
            homingMissile.GetComponent<HomingMissile>().SetBulletPool(m_homingMissilePool);
            m_homingMissileList.Add(homingMissile.GetComponent<HomingMissile>());
        }

        //레일건
        m_railGunDir = Quaternion.identity;
        m_railGunLevel = 1;
        m_railGunList = new List<Railgun>();
        for(int i = 0; i < m_railGunPoolSize; i++)
        {
            GameObject railgun = Instantiate(m_pRailgun, m_railGunPool);
            railgun.SetActive(false);
            railgun.GetComponent<Railgun>().SetBulletPool(m_railGunPool);
            m_railGunList.Add(railgun.GetComponent<Railgun>());
        }

        //드래곤브레스
        m_dragonBreathLevel = 1;
        GameObject dragonBreath = Instantiate(m_pDragonBreath, m_dragonBreathPool);
        dragonBreath.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        dragonBreath.GetComponent<DragonBreath>().SetBulletPool(m_dragonBreathPool);
        dragonBreath.SetActive(false);
        m_dragonBreath = dragonBreath.GetComponent<DragonBreath>();
    }

    public void FireHomingMissile()
    {
        //총알이 없어도 사운드가 재생 되니 나중에 수정
        SoundController.instance.PlaySFX(m_homingMissileSFX);

        switch (m_homingMissileLevel)
        {
            case 1:
                FireHomingLevel1();
                break;
            case 2:
                FireHomingLevel2();
                break;
            case 3:
                FireHomingLevel3();
                break;
        }
    }

    private void FireHomingLevel1()
    {
        int cnt = 0;

        //Transform[] targets = GameManager.instance.enermyPool.FindClosedEnermy(m_firePos.position, 2);
        Transform[] targets = FindTarget(m_firePos.position, 2);

        foreach (HomingMissile h in m_homingMissileList)
        {
            if (cnt >= 2)
                break;

            if(!h.IsFire)
            {
                switch(cnt)
                {
                    case 0:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, 45f);
                        break;
                    case 1:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, -45f);
                        break;
                }
                SetFireHoming(h, m_homingMissileDir, targets[cnt]);
                cnt++;
            }
        }
    }

    private void FireHomingLevel2()
    {
        int cnt = 0;

        //Transform[] targets = GameManager.instance.enermyPool.FindClosedEnermy(m_firePos.position, 4);
        Transform[] targets = FindTarget(m_firePos.position, 4);

        foreach (HomingMissile h in m_homingMissileList)
        {
            if (cnt >= 4)
                break;

            if (!h.IsFire)
            {
                switch (cnt)
                {
                    case 0:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, 30f);
                        break;
                    case 1:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, -30f);
                        break;
                    case 2:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, 60f);
                        break;
                    case 3:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, -60f);
                        break;

                }
                SetFireHoming(h, m_homingMissileDir, targets[cnt]);
                cnt++;
            }
        }
    }

    private void FireHomingLevel3()
    {
        int cnt = 0;

        //Transform[] targets = GameManager.instance.enermyPool.FindClosedEnermy(m_firePos.position, 7);
        Transform[] targets = FindTarget(m_firePos.position, 7);

        foreach (HomingMissile h in m_homingMissileList)
        {
            if (cnt >= 7)
                break;

            if (!h.IsFire)
            {
                switch (cnt)
                {
                    case 0:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, 30f);
                        break;
                    case 1:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, 60f);
                        break;
                    case 2:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, 90f);
                        break;
                    case 3:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, 0f);
                        break;
                    case 4:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, -30f);
                        break;
                    case 5:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, -60f);
                        break;
                    case 6:
                        m_homingMissileDir = Quaternion.Euler(0f, 0f, -90f);
                        break;

                }
                SetFireHoming(h, m_homingMissileDir, targets[cnt]);
                cnt++;
            }
        }
    }

    public Transform[] FindTarget(Vector3 start, int size)
    {
        List<Transform> targets = new List<Transform>();

        List<Transform> enermys = GameManager.instance.enermyPool.GetActiveEnermy();
        List<Transform> comets = CometController.instance.GetActiveComet();
        //List<Transform> fragments = CometController.instance.GetActiveFragment();

        targets.AddRange(enermys);
        targets.AddRange(comets);
        //targets.AddRange(fragments);

        float minDistance = 1000f;
        Transform[] results = new Transform[size];

        if (targets.Count <= 0)
            return results;

        if (targets.Count < size)
        {
            int cnt = 0;
            int enermyLen = targets.Count;
            while (cnt < size)
            {
                results[cnt] = targets[cnt % enermyLen].transform;
                cnt++;
            }

            return results;
        }

        for (int i = 0; i < size; i++)
        {
            minDistance = 1000f;
            Transform closedEnermy = null;

            foreach (Transform t in targets)
            {
                CharacterStatus stat = t.GetComponent<CharacterStatus>();

                if (stat)
                {
                    if (!stat.IsLockOn)
                    {
                        float distance = Vector3.Distance(start, t.transform.position);

                        if (distance < minDistance)
                        {
                            if (closedEnermy != null)
                                closedEnermy.GetComponent<CharacterStatus>().IsLockOn = false;

                            minDistance = distance;
                            results[i] = t.transform;
                            stat.IsLockOn = true;
                            closedEnermy = t;
                        }
                    }
                }
            }
        }


        foreach (Transform t in targets)
        {
            if(t.GetComponent<CharacterStatus>())
                t.GetComponent<CharacterStatus>().IsLockOn = false;
        }

        return results;
    }

    public void RailGunFire()
    {
        foreach(Railgun r in m_railGunList)
        {
            if(!r.IsFire)
            {
                SoundController.instance.PlaySFX(m_railGunSFX);
                r.transform.SetParent(null);
                r.Fire(m_firePos, Quaternion.identity);
                break;
            }
        }
    }

    public void DragonBreathFire()
    {
        if(!m_dragonBreath.IsFire)
        {
            SoundController.instance.PlaySFX(m_dragonBreathSFX);
            m_dragonBreath.Fire(m_firePos, Quaternion.identity);
            m_elapsedTime = 0f;
        }
    }

    private void SetFireHoming(HomingMissile h, Quaternion fireAngle, Transform target)
    {
        h.transform.SetParent(null);
        h.Fire(m_firePos, fireAngle);
        h.m_firePos = this.m_firePos;
        h.Target = target;
    }

    private void Update()
    {
        if(m_dragonBreath.IsFire)
        {
            m_elapsedTime += timeManager.GetTimeScale();

            if(m_elapsedTime >= m_dragonBreathDurationTime)
            {
                m_dragonBreath.InitBullet();
            }
        }
    }

    public void HomingMissileUpgrade()
    {
        float maxLevel = GameManager.instance.SubWeaponMaxLevel;
        if (m_homingMissileLevel >= maxLevel)
            return;

        m_homingMissileLevel++;

        foreach (HomingMissile h in m_homingMissileList)
        {
            h.Upgrade();
        }
    }

    public void RailGunUpgrade()
    {
        float maxLevel = GameManager.instance.SubWeaponMaxLevel;
        if (m_railGunLevel >= maxLevel)
            return;

        m_railGunLevel++;

        foreach (Railgun r in m_railGunList)
        {
            r.Upgrade();
        }
    }

    public void DragonBreathUpgrade()
    {
        float maxLevel = GameManager.instance.SubWeaponMaxLevel;
        if (m_dragonBreathLevel >= maxLevel)
            return;

        m_dragonBreathLevel++;

        m_dragonBreath.Upgrade();
    }
}
