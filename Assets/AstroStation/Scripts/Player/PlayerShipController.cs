using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainWeaponType
{
    Vulcan,
    Beam,
    Plasma
}

public enum SubWeaponType
{
    RailGun,
    HomingMissile,
    DragonBreath
}

public class PlayerShipController : MonoBehaviour
{
    //플레이어
    [Header("PlayerInfo")]
    [SerializeField] private CharacterStatus m_playerHp;
    private bool m_isGameOver;
    public GameObject m_shipModel;
    public float m_moveSpeed;
    public Transform m_screen;
    private HmdAngleTracker m_hmd;
    public bool VRMode = true;

    //임시
    //public GameObject m_beamParticle;

    //무기 관련
    [Header("Weapon")]
    public BulletPool m_bulletPool;
    public SubWeaponPool m_subWeaponPool;
    [SerializeField] private MainWeaponType m_mainWeaponType;
    [SerializeField] private SubWeaponType m_subWeaponType;
    private int m_subWeaponNumber;
    //폭탄
    public Bomb m_bomb;

    public bool IsLayoutA;

    //이펙트
    [Header("Effect")]
    public GameObject m_shipDestroyEff;

    //사운드
    [Header("Sound")]
    public AudioClip m_shipDestroySFX;
    public AudioClip m_subWeaponFireFailSFX;


    private void Awake()
    {
        //GameManager.instance.GameManagerInit("astro_hard_sample");

        //플레이어, 카메라 위치 세팅
        m_playerHp = this.GetComponent<CharacterStatus>();
        m_isGameOver = false;
        this.transform.position = new Vector3(0, 32f / 5f, 0f);
        this.GetComponentInChildren<Camera>().transform.position = new Vector3(0f, 32f / 10f * 5f, -10f);

        //무기 세팅
        if (!m_bulletPool)
            m_bulletPool = this.GetComponentInChildren<BulletPool>();
        if (!m_subWeaponPool)
            m_subWeaponPool = this.GetComponentInChildren<SubWeaponPool>();
        m_subWeaponNumber = 0;

        //if (m_beamParticle)
        //    m_beamParticle.SetActive(false);

        if (!m_bomb)
            m_bomb = this.GetComponentInChildren<Bomb>();

        m_hmd = this.GetComponent<HmdAngleTracker>();

        //2D모드 세팅
        if (!VRMode)
        {
            this.GetComponentInChildren<Camera>().targetTexture = null;
            this.GetComponentInChildren<AudioListener>().enabled = true;
            m_hmd.cam.root.gameObject.SetActive(false);
            m_moveSpeed = 16f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //HP 업데이트
        StatusUpdate();

        //데스크탑 컨트롤
        if (!VRMode)
        {
            float moveX = Input.GetAxis("Horizontal");
            if (moveX != 0f)
            {
                this.transform.Translate(new Vector3(moveX, 0f, 0f).normalized * Time.deltaTime * m_moveSpeed);
                GameManager.instance.PlayerShipPositionCheck();
            }
        }
        //VR 컨트롤
        else
        {
            //우주선은 x방향으로 움직이고 원통은 y축을 기준으로 같이 회전
            Vector3 moveX = m_hmd.MoveX;
            float fMoveX = m_hmd.FloatMoveX;
            if (moveX != Vector3.zero)
            {
                this.transform.Translate(moveX * m_moveSpeed);
                m_screen.transform.Rotate(Vector3.up, fMoveX);

                if (fMoveX < 0)
                {
                    if (fMoveX < -0.1f)
                        m_shipModel.transform.rotation = Quaternion.Euler(270f, 40f, 0f);
                    else
                        m_shipModel.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                }
                else
                {
                    if(fMoveX > 0.1f)
                        m_shipModel.transform.rotation = Quaternion.Euler(270f, -40f, 0f);
                    else
                        m_shipModel.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                }

                GameManager.instance.PlayerShipPositionCheck();
            }
        }

        if (m_isGameOver)
            return;


        

        //무기 발사
        //float subWeaponShootTiming = Mathf.Lerp(0f, 1f, TimeManager.instance.ElapsedTime / TimeManager.instance.SubWeaponTiming);
        ////Debug.Log(TimeManager.instance.GetElapsedTime() / TimeManager.instance.GetSubWeaponTiming());
        //if (subWeaponShootTiming >= 0.8f && subWeaponShootTiming <= 1f)
        //{
        //    this.GetComponent<SpriteRenderer>().color = new Color(subWeaponShootTiming, 0f, 0f);
        //    //Debug.Log("지금 타이밍!");
        //}
        //else
        //{
        //    this.GetComponent<SpriteRenderer>().color = Color.white;
        //}

        //Debug.Log(TimeManager.instance.ElapsedTime);

        //보조무기 발사타이밍 시각화
        float elapsedTime = TimeManager.instance.ElapsedTime;
        float startTime = TimeManager.instance.SubWeaponTiming - TimeManager.instance.PerfectTime;
        float endTime = TimeManager.instance.SubWeaponTiming + TimeManager.instance.PerfectTime;

        if (elapsedTime > startTime && elapsedTime < endTime)
        {
            m_shipModel.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            m_shipModel.GetComponent<MeshRenderer>().material.color = Color.white;
        }


        if (!VRMode)
        {
            //주무장
            if (Input.GetKey(KeyCode.LeftControl))
            {
                MainWeaponFire();
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                DisableMainWeapon();
            }

            //보조무장
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                //if (subWeaponShootTiming >= 0.8f && subWeaponShootTiming <= 1f
                //    && TimeManager.instance.enableSubWeaponFire)
                //{
                //    SubWeaponFire();
                //    TimeManager.instance.enableSubWeaponFire = false;
                //}
                bool isSubWeaponFire = false;

                //Miss
                if (TimeManager.instance.enableSubWeaponFire)
                {
                    if (TimeManager.instance.ElapsedTime > TimeManager.instance.SubWeaponTiming - TimeManager.instance.FrontMissTime &&
                        TimeManager.instance.ElapsedTime < TimeManager.instance.SubWeaponTiming + TimeManager.instance.BackMissTime)
                    {
                        //perfect
                        if (TimeManager.instance.ElapsedTime >= TimeManager.instance.SubWeaponTiming - TimeManager.instance.PerfectTime &&
                            TimeManager.instance.ElapsedTime <= TimeManager.instance.SubWeaponTiming + TimeManager.instance.PerfectTime)
                        {
                            isSubWeaponFire = true;
                            SubWeaponFire();
                        }
                        TimeManager.instance.enableSubWeaponFire = false;
                    }
                }

                //발사 실패 사운드
                if(!isSubWeaponFire)
                {
                    SoundController.instance.PlaySFX(m_subWeaponFireFailSFX);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SubWeaponChange();
            }

            //폭탄
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_bomb.BombExplode();
            }
        }
        else
        {
            if (this.IsLayoutA == true)
            {
                //주무장발사
                if (QuestController.instance.LeftTriggerInputValue() >= 0.1f)
                {
                    MainWeaponFire();
                }
                else
                {
                    DisableMainWeapon();
                    WearAbleManager.instance.MainWeaponNotFire();
                }

                //보조무장 발사
                if (QuestController.instance.RightTriggerDown())
                {
                    //if (subWeaponShootTiming >= 0.8f && subWeaponShootTiming <= 1f 
                    //    && TimeManager.instance.enableSubWeaponFire)
                    //{
                    //    SubWeaponFire();
                    //    QuestController.instance.Vibration(2f, 0.1f, 0.5f, OVRInput.Controller.RTouch);
                    //}
                    bool isSubWeaponFire = false;

                    if (TimeManager.instance.enableSubWeaponFire)
                    {
                        if (TimeManager.instance.ElapsedTime > TimeManager.instance.SubWeaponTiming - TimeManager.instance.FrontMissTime &&
                            TimeManager.instance.ElapsedTime < TimeManager.instance.SubWeaponTiming + TimeManager.instance.BackMissTime)
                        {
                            //perfect
                            if (TimeManager.instance.ElapsedTime >= TimeManager.instance.SubWeaponTiming - TimeManager.instance.PerfectTime &&
                                TimeManager.instance.ElapsedTime <= TimeManager.instance.SubWeaponTiming + TimeManager.instance.PerfectTime)
                            {
                                isSubWeaponFire = true;
                                SubWeaponFire();
                            }
                            TimeManager.instance.enableSubWeaponFire = false;
                        }
                    }
                    //발사 실패 사운드
                    if (!isSubWeaponFire)
                    {
                        SoundController.instance.PlaySFX(m_subWeaponFireFailSFX);
                    }
                }
                else
                {
                    WearAbleManager.instance.SubWeaponNotFire();
                }

                if (QuestController.instance.RightGripDown())
                {
                    SubWeaponChange();
                    //QuestController.instance.Vibration(2f, 0.1f, 0.5f, OVRInput.Controller.RTouch);
                }

                //폭탄
                if (QuestController.instance.LeftGripDown())
                {
                    m_bomb.BombExplode();
                }
            }
            else
            {
                //주무장발사
                if (QuestController.instance.RightTriggerInputValue() >= 0.1f)
                {
                    MainWeaponFire();
                }
                else
                {
                    DisableMainWeapon();
                }

                //보조무장 발사
                if (QuestController.instance.LeftTriggerDown())
                {
                    //if (subWeaponShootTiming >= 0.8f && subWeaponShootTiming <= 1f 
                    //    && TimeManager.instance.enableSubWeaponFire)
                    //{
                    //    SubWeaponFire();
                    //    QuestController.instance.Vibration(2f, 0.1f, 0.5f, OVRInput.Controller.RTouch);
                    //}
                    if (TimeManager.instance.enableSubWeaponFire)
                    {
                        if (TimeManager.instance.ElapsedTime > TimeManager.instance.SubWeaponTiming - TimeManager.instance.FrontMissTime &&
                            TimeManager.instance.ElapsedTime < TimeManager.instance.SubWeaponTiming + TimeManager.instance.BackMissTime)
                        {
                            //perfect
                            if (TimeManager.instance.ElapsedTime >= TimeManager.instance.SubWeaponTiming - TimeManager.instance.PerfectTime &&
                                TimeManager.instance.ElapsedTime <= TimeManager.instance.SubWeaponTiming + TimeManager.instance.PerfectTime)
                            {
                                SubWeaponFire();
                            }
                            TimeManager.instance.enableSubWeaponFire = false;
                        }
                    }
                }

                if (QuestController.instance.LeftGripDown())
                {
                    SubWeaponChange();
                    //QuestController.instance.Vibration(2f, 0.1f, 0.5f, OVRInput.Controller.RTouch);
                }

                //폭탄
                if (QuestController.instance.RightGripDown())
                {
                    m_bomb.BombExplode();
                }
            }

        }

    }

    public void MainWeaponUpgrade(MainWeaponType weaponType)
    {
        m_mainWeaponType = weaponType;
        switch (m_mainWeaponType)
        {
            case MainWeaponType.Vulcan:
                m_bulletPool.VulcanUpgrade();
                m_bulletPool.DisableBeam();
                //if (m_beamParticle)
                //    m_beamParticle.SetActive(false);
                m_bulletPool.DisablePlasma();
                break;
            case MainWeaponType.Beam:
                m_bulletPool.BeamUpgrade();
                break;
            case MainWeaponType.Plasma:
                break;
        }
    }

    public void SubWeaponUpgrade(SubWeaponType weaponType)
    {
        m_subWeaponType = weaponType;
        switch (m_subWeaponType)
        {
            case SubWeaponType.RailGun:
                m_subWeaponNumber = 0;
                m_subWeaponPool.RailGunUpgrade();
                break;
            case SubWeaponType.DragonBreath:
                m_subWeaponNumber = 1;
                m_subWeaponPool.DragonBreathUpgrade();
                break;
            case SubWeaponType.HomingMissile:
                m_subWeaponNumber = 2;
                m_subWeaponPool.HomingMissileUpgrade();
                break;
        }
    }


    private void MainWeaponFire()
    {
        switch (m_mainWeaponType)
        {
            case MainWeaponType.Vulcan:
                m_bulletPool.FireVulcan();
                WearAbleManager.instance.VulcanFire();
                break;
            case MainWeaponType.Beam:
                m_bulletPool.FireBeam();
                WearAbleManager.instance.LaserBeamFire();
                //if (m_beamParticle)
                //    m_beamParticle.SetActive(true);
                break;
            case MainWeaponType.Plasma:
                m_bulletPool.FirePlasma();
                break;
        }
    }

    private void DisableMainWeapon()
    {
        switch (m_mainWeaponType)
        {
            case MainWeaponType.Vulcan:
                break;
            case MainWeaponType.Beam:
                m_bulletPool.DisableBeam();
                //if (m_beamParticle)
                //    m_beamParticle.SetActive(false);
                break;
            case MainWeaponType.Plasma:
                m_bulletPool.DisablePlasma();

                break;
        }
    }

    private void SubWeaponFire()
    {
        switch (m_subWeaponType)
        {
            case SubWeaponType.RailGun:
                m_subWeaponPool.RailGunFire();
                WearAbleManager.instance.RailGunFire();
                break;
            case SubWeaponType.DragonBreath:
                m_subWeaponPool.DragonBreathFire();
                WearAbleManager.instance.DragonBreathFire();
                break;
            case SubWeaponType.HomingMissile:
                m_subWeaponPool.FireHomingMissile();
                WearAbleManager.instance.HomingMissileFire();
                break;
        }
    }

    private void SubWeaponChange()
    {
        m_subWeaponNumber++;
        int sw = m_subWeaponNumber % 3;
        switch (sw)
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

    private void StatusUpdate()
    {
        if (m_isGameOver)
            return;

        if (m_playerHp.hp <= 0)
        {
            m_isGameOver = true;

            m_shipModel.SetActive(false);
            DisableMainWeapon();
            this.GetComponent<Collider2D>().enabled = false;

            if (m_shipDestroyEff)
            {
                GameObject eff = Instantiate(m_shipDestroyEff, null);
                eff.transform.position = this.transform.position;
                Destroy(eff, 3f);
            }

            SoundController.instance.PlaySFX(m_shipDestroySFX);

            GameManager.instance.InGameUI.GetComponent<InGameUI>().OpenDefeatUI();
        }
    }

    public void PlayerReset()
    {
        m_isGameOver = false;
        m_shipModel.SetActive(true);
        this.GetComponent<Collider2D>().enabled = true;
        m_playerHp.HpPlus(3f);
    }
}