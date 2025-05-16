using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Weapon
{
    public enum BeamFirePos
    {
        right,
        left
    }

    //private float m_fDis = 0f;
    //private bool m_isHit = false;

    //public GameObject m_hitBeamEffect;
    //private GameObject m_hitEff;

    //[SerializeField] Transform LeftBeam_L;
    //[SerializeField] Transform LeftBeam_C;
    //[SerializeField] Transform LeftBeam_R;

    //[SerializeField] Transform RightBeam_L;
    //[SerializeField] Transform RightBeam_C;
    //[SerializeField] Transform RightBeam_R;

    //[SerializeField] Transform skyBeam;
    //[SerializeField] Transform whiteBeam;
    //private float m_skyBeamDefaultWidth;
    //private float m_whiteBeamDefaultWidth;
    //[SerializeField] private float m_beamWarmUpTime;
    //private float m_warmUpElapsedTime;

    //private float m_beamSize = 0.1f;

    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab; //Put a prefab with a line renderer onto here.
    public GameObject beamStartPrefab; //This is a prefab that is put at the start of the beam.
    public GameObject beamEndPrefab; //Prefab put at end of beam.

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Beam Options")]
    public bool alwaysOn = true; //Enable this to spawn the beam when script is loaded.
    public bool beamCollides = true; //Beam stops at colliders
    public float beamLength = 100; //Ingame beam length
    public float beamEndOffset = 0f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. 
                                            //Example: if texture is 200 pixels in height and 600 in length, set this to 3

    private BeamFirePos m_beamFirePos;

    private void OnEnable()
    {
        if (alwaysOn) //When the object this script is attached to is enabled, spawn the beam.
            SpawnBeam();
    }

    private void OnDisable() //If the object this script is attached to is disabled, remove the beam.
    {
        RemoveBeam();
    }


    public override void Awake()
    {
        base.Awake();
        m_weaponLevel = 1f;
        m_damage = 2f;
        //m_beamSize = skyBeam.localScale.y;

        //m_skyBeamDefaultWidth = skyBeam.localScale.y;
        //m_whiteBeamDefaultWidth = whiteBeam.localScale.y;
        //m_warmUpElapsedTime = 0f;
    }

    private void Update()
    {
        //if(!m_isFire)
        //    return;

        //if (m_isHit)
        //{
        //    this.transform.localScale = new Vector3(1f, m_fDis, 1f);
        //}
        //else
        //{
        //    //float move = m_speed * timeManager.GetTimeScale();
        //    //m_moveDistance += move;

        //    this.transform.localScale = new Vector3(1f, m_maximumRange, 1f);
        //}

        ////
        //m_warmUpElapsedTime += timeManager.GetTimeScale();

        //float whiteWidth = Mathf.Lerp(0, m_beamSize * (m_weaponLevel - 1), m_warmUpElapsedTime / m_beamWarmUpTime);
        //float skyWidth = Mathf.Lerp(0.01f, m_beamSize * m_weaponLevel, m_warmUpElapsedTime / m_beamWarmUpTime);

        //whiteBeam.localScale = new Vector3(0.125f, whiteWidth, 1f);
        //skyBeam.localScale = new Vector3(0.125f, skyWidth, 1f);
        //this.GetComponent<BoxCollider2D>().size = new Vector2(skyWidth, 1f);
    }

    void FixedUpdate()
    {
        if (beam && m_isFire) //Updates the beam
        {
            line.SetPosition(0, transform.position);

            Vector3 end;
            RaycastHit2D hit;
            CharacterStatus stat = null;
            int layerMask = (1 << LayerMask.NameToLayer("2D"));
            hit = Physics2D.Raycast(transform.position, transform.up, m_maximumRange, layerMask);

            if(hit)
                stat = hit.transform.GetComponent<CharacterStatus>();


            if (beamCollides && stat) //Checks for collision
            {
                end = (Vector3)hit.point - (transform.up * beamEndOffset);
                if(stat)
                {
                    stat.Damaged(m_damage * timeManager.GetMeasureTime() * timeManager.GetTimeScale());
                }
            }
            else
                end = transform.position + (transform.up * beamLength);

            line.SetPosition(1, end);

            if (beamStart)
            {
                beamStart.transform.position = transform.position;
                beamStart.transform.LookAt(end);
            }
            if (beamEnd)
            {
                beamEnd.transform.position = end;
                beamEnd.transform.LookAt(beamStart.transform.position);
            }

            float distance = Vector3.Distance(transform.position, end);
            line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
            line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
        }
    }

    public override void Upgrade()
    {
        float maxLevel = GameManager.instance.MainWeaponMaxLevel;

        if (m_weaponLevel >= maxLevel)
            return;

        //if(!LeftBeam_C || !LeftBeam_L || !LeftBeam_R || !RightBeam_C || !RightBeam_L || !RightBeam_R)
        //{
        //    return;
        //}

        //float size = 0.05f;
        //float halfSize = 0.025f;
        //LeftBeam_C.localScale = new Vector3(1f, m_weaponLevel * size, 1f);
        //LeftBeam_L.localPosition = new Vector3(-size - (halfSize * m_weaponLevel), 0f, 0f);
        //LeftBeam_R.localPosition = new Vector3(size + (halfSize * m_weaponLevel), 0f, 0f);

        //RightBeam_C.localScale = new Vector3(1f, m_weaponLevel * size, 1f);
        //RightBeam_L.localPosition = new Vector3(-size - (halfSize * m_weaponLevel), 0f, 0f);
        //RightBeam_R.localPosition = new Vector3(size + (halfSize * m_weaponLevel), 0f, 0f);

        m_weaponLevel++;

        //whiteBeam.localScale = new Vector3(0.125f, m_beamSize * (m_weaponLevel - 1), 1f);
        //skyBeam.localScale = new Vector3(0.125f, m_beamSize * m_weaponLevel, 1f);
        //this.GetComponent<BoxCollider2D>().size = new Vector2(m_beamSize * m_weaponLevel, 1f);


        if (m_weaponLevel == 2)
        {
            m_damage = 2f;
        }
        else if(m_weaponLevel == 3)
        {
            m_damage = 3f;
        }
        else if(m_weaponLevel == 4)
        {
            m_damage = 5f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        //    return;

        //CharacterStatus stat = collision.GetComponent<CharacterStatus>();

        //if (stat)
        //{
        //    if (m_hitEff)
        //        Destroy(m_hitEff);

        //    if (m_hitBeamEffect)
        //    {
        //        m_hitEff = Instantiate(m_hitBeamEffect);
        //        m_hitEff.transform.position = collision.transform.position;
        //    }
        //}
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        CharacterStatus stat = collision.GetComponent<CharacterStatus>();

        if (stat)
        {
            m_isHit = true;
            m_fDis = Vector3.Distance(this.transform.position, collision.transform.position);
            m_fDis -= (collision.bounds.size.y / 2);

            //콜라이더의 크기가 너무 작아지면 트리거가 작동하지 않는다
            if (m_fDis < 0.01f)
                m_fDis = 0.01f;

            //데미지 처리 부분을 업데이트로 옮겨야 할지 생각
            collision.GetComponent<CharacterStatus>().Damaged(m_damage * timeManager.GetMeasureTime() * timeManager.GetTimeScale());

            if (m_hitBeamEffect)
            {
                m_hitBeamEffect.SetActive(true);
                m_hitBeamEffect.transform.position = collision.transform.position;
            }
            //m_elapsedDot = timeManager.GetDamageOverTime();
        }
    }
    */
    
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        //CharacterStatus stat = collision.GetComponent<CharacterStatus>();

        //if (stat)
        //{
        //    m_isHit = false;
        //    m_moveDistance = m_fDis;
        //    m_fDis = 0f;
        //    if (m_hitBeamEffect)
        //    {
        //        Destroy(m_hitEff);
        //    }
        //}

        m_isHit = false;
        m_moveDistance = m_fDis;
        m_fDis = 0f;
        m_hitBeamEffect.SetActive(false);

        //if (m_hitBeamEffect)
        //{
        //    Destroy(m_hitEff);
        //}
    }

    */

    public override void Fire(Transform firePos, Quaternion dir)
    {
        m_isFire = true;
        this.transform.localRotation = dir;

        Vector3 pos = firePos.transform.localPosition;

        if (m_beamFirePos == BeamFirePos.left)
            this.transform.localPosition = new Vector3(-0.5f, pos.y, pos.z);
        else
            this.transform.localPosition = new Vector3(0.5f, pos.y, pos.z);

        this.gameObject.SetActive(true);
    }

    public override void InitBullet()
    {
        base.InitBullet();
        /*
        m_isHit = false;
        m_fDis = 0f;
        m_hitBeamEffect.SetActive(false);

        whiteBeam.localScale = new Vector3(0.125f, 0, 1f);
        skyBeam.localScale = new Vector3(0.125f, 0, 1f);
        this.GetComponent<BoxCollider2D>().size = new Vector2(0, 1f);
        m_warmUpElapsedTime = 0f;
        */
    }

    public BeamFirePos FirePos
    {
        get { return m_beamFirePos; }
        set { m_beamFirePos = value; }
    }

    public void SetBeamHitEffect(GameObject eff)
    {
        //m_hitBeamEffect = eff;
        //m_hitBeamEffect.SetActive(false);
    }

    public void SpawnBeam() //This function spawns the prefab with linerenderer
    {
        if (beamLineRendererPrefab)
        {
            if (beamStartPrefab)
                beamStart = Instantiate(beamStartPrefab);
            if (beamEndPrefab)
                beamEnd = Instantiate(beamEndPrefab);
            beam = Instantiate(beamLineRendererPrefab);
            beam.transform.position = transform.position;
            beam.transform.parent = transform;
            beam.transform.rotation = transform.rotation;
            line = beam.GetComponent<LineRenderer>();
            line.useWorldSpace = true;
#if UNITY_5_5_OR_NEWER
            line.positionCount = 2;
#else
			line.SetVertexCount(2); 
#endif
        }
        else
            print("Add a hecking prefab with a line renderer to the PixelArsenalBeamStatic script on " + gameObject.name + "! Heck!");
    }

    public void RemoveBeam() //This function removes the prefab with linerenderer
    {
        if (beam)
            Destroy(beam);
        if (beamStart)
            Destroy(beamStart);
        if (beamEnd)
            Destroy(beamEnd);
    }
}
