using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public int m_poolSize;
    public float m_fireDelay = 1f;
    public Transform m_firePos;

    private TimeManager m_timeManager;
    private float m_elapsedFireTime = 0f;
    private Quaternion m_bulletDir;

    //발칸
    public GameObject m_pVulcan;
    public Transform m_vulcanPool;
    private List<Vulcan> m_vulcanList;
    [SerializeField]private float m_vulcanLevel;
    [SerializeField] private AudioClip m_vulcanSFX;

    //빔
    public GameObject m_pBeam;
    //public GameObject m_pBeamHitEffect;
    public Transform m_beamPool;
    private Beam m_leftBeam;
    private Beam m_rightBeam;
    [SerializeField] private float m_beamLevel;
    [SerializeField] private AudioClip m_beamSFX;

    //플라즈마
    public GameObject m_pPlasma;
    public Transform m_plasmaPool;
    private Plasma m_plasma;
    [SerializeField] private float m_plasmaLevel;


    private void Awake()
    {
        m_timeManager = TimeManager.instance;
        m_bulletDir = Quaternion.identity;

        InitBullets();
    }

    private void Update()
    {
        if (m_elapsedFireTime > 0f)
            m_elapsedFireTime -= m_timeManager.GetTimeScale();
        else
            m_elapsedFireTime = 0f;
    }

    private void InitBullets()
    {
        m_elapsedFireTime = 0f;
        //발칸 생성및초기화
        m_vulcanLevel = 1;
        m_vulcanList = new List<Vulcan>();

        for(int i = 0; i < m_poolSize; i++)
        {
            GameObject vulcan = Instantiate(m_pVulcan, m_vulcanPool);
            vulcan.gameObject.SetActive(false);
            vulcan.GetComponent<Vulcan>().SetBulletPool(m_vulcanPool);

            m_vulcanList.Add(vulcan.GetComponent<Vulcan>());
        }

        //빔 생성및초기화
        m_beamLevel = 1f;
        GameObject beams;
        GameObject beamEff;

        beams = Instantiate(m_pBeam, m_beamPool);
        //beamEff = Instantiate(m_pBeamHitEffect, m_beamPool);
        beams.transform.localPosition = new Vector3(-0.5f, 0f, 0f);
        beams.name = "LeftBeam";
        beams.GetComponent<Beam>().SetBulletPool(m_beamPool);
        m_leftBeam = beams.GetComponent<Beam>();
        m_leftBeam.m_firePos = m_firePos;
        m_leftBeam.FirePos = Beam.BeamFirePos.left;
        //m_leftBeam.SetBeamHitEffect(beamEff);
        beams.SetActive(false);

        beams = Instantiate(m_pBeam, m_beamPool);
        //beamEff = Instantiate(m_pBeamHitEffect, m_beamPool);
        beams.transform.localPosition = new Vector3(0.5f, 0f, 0f);
        beams.name = "RightBeam";
        beams.GetComponent<Beam>().SetBulletPool(m_beamPool);
        m_rightBeam = beams.GetComponent<Beam>();
        m_rightBeam.m_firePos = m_firePos;
        m_rightBeam.FirePos = Beam.BeamFirePos.right;
        //m_rightBeam.SetBeamHitEffect(beamEff);
        beams.SetActive(false);

        //플라즈마 생성및 초기화
        m_plasmaLevel = 1f;
        GameObject plasma;

        plasma = Instantiate(m_pPlasma, m_plasmaPool);
        plasma.transform.localPosition = Vector3.zero;
        plasma.name = "Plasma";
        plasma.GetComponent<Plasma>().SetBulletPool(m_plasmaPool);
        m_plasma = plasma.GetComponent<Plasma>();
        m_plasma.m_firePos = m_firePos;
        plasma.SetActive(false);

    }

    public void FireVulcan()
    {
        if (m_elapsedFireTime > 0f)
            return;

        SoundController.instance.PlaySFX(m_vulcanSFX);

        VulcanLevel1();

        if(m_vulcanLevel >= 2)
        {
            VulcanLevel2();
        }
        if (m_vulcanLevel >= 3)
        {
            VulcanLevel3();
        }
        if (m_vulcanLevel >= 4)
        {
            VulcanLevel4();
        }
    }

    public void FireBeam()
    {
        BeamLevel1();
    }

    public void DisableBeam()
    {
        SoundController.instance.StopSFXClipName(m_beamSFX.name);
        m_leftBeam.InitBullet();
        m_rightBeam.InitBullet();
    }

    public void FirePlasma()
    {
        m_plasma.Fire(m_firePos, Quaternion.identity);
    }

    public void DisablePlasma()
    {
        m_plasma.InitBullet();
    }

    private void VulcanLevel1()
    {
        foreach (Vulcan bv in m_vulcanList)
        {
            if (!bv.IsFire)
            {
                m_bulletDir = Quaternion.identity;
                SetFireVulcan(bv, m_bulletDir.normalized);
                break;
            }
        }
    }
    private void VulcanLevel2()
    {
        int n = 0;
        foreach (Vulcan bv in m_vulcanList)
        {
            if (!bv.IsFire)
            {
                if (n == 0)
                {
                    m_bulletDir = Quaternion.Euler(0f, 0f, 15f);

                    SetFireVulcan(bv, m_bulletDir.normalized);
                    n++;
                }
                else if (n == 1)
                {
                    m_bulletDir = Quaternion.Euler(0f, 0f, -15f);

                    SetFireVulcan(bv, m_bulletDir.normalized);
                    break;
                }

            }
        }
    }
    private void VulcanLevel3()
    {
        int n = 0;
        foreach (Vulcan bv in m_vulcanList)
        {
            if (!bv.IsFire)
            {
                if (n == 0)
                {
                    m_bulletDir = Quaternion.Euler(0f, 0f, 30f);

                    SetFireVulcan(bv, m_bulletDir.normalized);
                    n++;
                }
                else if (n == 1)
                {
                    m_bulletDir = Quaternion.Euler(0f, 0f, -30f);

                    SetFireVulcan(bv, m_bulletDir.normalized);
                    break;
                }

            }
        }
    }

    private void VulcanLevel4()
    {
        int n = 0;
        foreach (Vulcan bv in m_vulcanList)
        {
            if (!bv.IsFire)
            {
                if (n == 0)
                {
                    m_bulletDir = Quaternion.Euler(0f, 0f, 45f);

                    SetFireVulcan(bv, m_bulletDir.normalized);
                    n++;
                }
                else if (n == 1)
                {
                    m_bulletDir = Quaternion.Euler(0f, 0f, -45f);

                    SetFireVulcan(bv, m_bulletDir.normalized);
                    break;
                }

            }
        }
    }

    private void SetFireVulcan(Vulcan bv, Quaternion fireAngle)
    {
        bv.transform.SetParent(null);
        bv.Fire(m_firePos, fireAngle);
        m_elapsedFireTime = m_fireDelay;
        bv.m_firePos = this.m_firePos; 
    }

    private void BeamLevel1()
    {
        if (!m_leftBeam.IsFire)
            SoundController.instance.PlaySFX(m_beamSFX, true);

        m_leftBeam.Fire(m_firePos, Quaternion.identity);
        m_rightBeam.Fire(m_firePos, Quaternion.identity);
    }

    public void VulcanUpgrade()
    {
        if (m_vulcanLevel >= GameManager.instance.MainWeaponMaxLevel)
            return;


        m_vulcanLevel++;

        foreach (Vulcan v in m_vulcanList)
        {
            v.Upgrade();
        }
    }

    public void BeamUpgrade()
    {
        m_leftBeam.Upgrade();
        m_rightBeam.Upgrade();
    }

}
