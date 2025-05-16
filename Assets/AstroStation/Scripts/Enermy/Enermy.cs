using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENERMYTYPE
{
    A,
    B,
    C
}

public class Enermy : MonoBehaviour
{
    public ENERMYTYPE m_type;

    public GameObject m_item;
    public GameObject m_explosionEff;
    private CharacterStatus m_stat;

    //미사일
    public Transform bulletFirePosition;
    private EnermyBulletPool m_bullet;

    private float m_fireDelay;
    private float m_fireElapsed;

    [SerializeField] private AudioClip explosionSFX;

    private bool isMove = false;
    private Vector3 moveDest;

    private float moveDistance;
    private float destDistance;

    private float speed = 1f;

    private bool m_isLockOn = false;

    private void Awake()
    {
        m_stat = this.GetComponent<CharacterStatus>();


        m_bullet = FindObjectOfType<EnermyBulletPool>();

        m_fireElapsed = 0f;
        m_fireDelay = Random.Range(1f, 5f);
        
        arrPos = new Vector3[5];


        //arrPos[0] = new Vector3(-28f, 31f, 0f);
        //arrPos[1] = new Vector3(-18f, 22f, 0f);
        //arrPos[2] = new Vector3(-10f, 29f, 0f);
        //arrPos[3] = new Vector3(0f, 20f, 0f);
        //arrPos[4] = new Vector3(12f, 29f, 0f);

        //arrPos[0] = new Vector3(-35f, 32f, 0f);
        //arrPos[1] = new Vector3(18f, 24f, 0f);
        //arrPos[2] = new Vector3(-20f, 13f, 0f);
        //arrPos[3] = new Vector3(34f, 7f, 0f);
        //arrPos[4] = new Vector3(5f, 0f, 0f);
        
    }

    private void Start()
    {
        //this.onPath();
    }

    private void Update()
    {
        if (m_stat.hp <= 0f)
        {
            ShipDestroy();
        }

        if(m_bullet && !isMove)
        {
            m_fireElapsed += TimeManager.instance.GetTimeScale();

            if (m_fireElapsed > m_fireDelay)
            {
                m_bullet.BulletFire(bulletFirePosition, Quaternion.identity);
                m_fireDelay = Random.Range(2f, 5f);
                m_fireElapsed = 0f;
            }
        }

        if (isMove)
        {
            if(moveDistance >= destDistance)
            {
                isMove = false;
                //MoveTo();
                moveDistance = 0f;
                return;
            }

            /*
            Vector3 dir = moveDest - this.transform.position;
            this.transform.Translate(dir.normalized * TimeManager.instance.GetTimeScale() * speed);
            moveDistance += Vector3.Magnitude(dir.normalized * TimeManager.instance.GetTimeScale() * speed);
            */

            //this.MoveBezierCurve();

          

            float bgBoundhalfSize = GameManager.instance.GetBgBoundSizeX() / 2f;
            float moveX = 0f;

            if (this.transform.position.x <= GameManager.instance.spaceBg[0].position.x - bgBoundhalfSize)
            {
               moveX = GameManager.instance.GetBgBoundSizeX() * 3f;

                if (moveDest.x != this.transform.position.x + moveX)
                {
                    MoveDestReset(moveX);
                }
                this.transform.Translate(moveX, 0f, 0f);
                for (int i = 0; i < arrPos.Length; i++)
                {
                    arrPos[i] = arrPos[i] + new Vector3(moveX, 0f, 0f);
                }
            }
            else if(this.transform.position.x >= GameManager.instance.spaceBg[2].position.x + bgBoundhalfSize)
            {
                moveX = GameManager.instance.GetBgBoundSizeX() * 3f;

                if (moveDest.x != this.transform.position.x - moveX)
                {
                    MoveDestReset(-moveX);
                }
                this.transform.Translate(-moveX, 0f, 0f);
                for (int i = 0; i < arrPos.Length; i++)
                {
                    arrPos[i] = arrPos[i] + new Vector3(-moveX, 0f, 0f);
                }
            }
            if(TimeManager.instance.GetTimeScale() != 0f)
            {
                this.elapsedTime = elapsedTime + Time.deltaTime;
                iTween.PutOnPath(this.gameObject, arrPos, this.elapsedTime / 40f);
            }

            if (this.elapsedTime > 40f)
            {
                isMove = false;
            }
        }
    }

    public void Init()
    {
        this.gameObject.SetActive(true);
        m_stat.hp = 1f;
        //this.transform.position = GameManager.instance.GetDestPosition();
        //MoveTo();

        isMove = true;
        elapsedTime = 0f;
        arrPos[0] = new Vector3(Random.Range(-48f, 48f), Random.Range(28f, 32f));
        arrPos[1] = new Vector3(Random.Range(-48f, 48f), Random.Range(10f, 32f));
        arrPos[2] = new Vector3(Random.Range(-48f, 48f), Random.Range(28f, 32f));
        arrPos[3] = new Vector3(Random.Range(-48f, 48f), Random.Range(10f, 32f));
        arrPos[4] = new Vector3(Random.Range(-48f, 48f), Random.Range(10f, 32f));
    }

    public void MoveTo()
    {
        isMove = true;
        moveDest = GameManager.instance.GetDestPosition();
        destDistance = Vector3.Distance(this.transform.position, moveDest);
    }

    public bool IsMove()
    {
        return isMove;
    }

    public void MoveDestReset(float moveX)
    {
        moveDest.x += moveX;
    }


    private void ShipDestroy()
    {
        this.gameObject.SetActive(false);
        isMove = false;

        //파괴이펙트 생성
        GameObject eff = Instantiate(m_explosionEff);
        eff.transform.position = this.transform.position;
        Destroy(eff, 2f);

        //폭발음 재생
        SoundController.instance.PlaySFX(explosionSFX);

        //아이템 생성
        if(m_item)
        {
            GameObject item = Instantiate(m_item);
            item.transform.position = this.transform.position;
            //item.GetComponent<WeaponItem>().MoveTo();
        }
    }

    public bool IsLockOn
    {
        get { return m_isLockOn; }
        set { m_isLockOn = value; }
    }

    public float Life
    {
        get { return m_stat.hp; }
        set { m_stat.hp = value; }
    }

    //  승룡 이동 구현중

    // 베지어 곡선
    public Vector3[] arrPos;
    public float time;

    public void MoveBezierCurve()
    {
        this.time += Time.deltaTime;
        this.transform.position = this.CalculateBezierPos(arrPos[0], arrPos[1], arrPos[2], arrPos[3], arrPos[4], this.time * 0.1f);
    }
    public Vector3 CalculateBezierPos(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        Vector3 v0 = Vector3.Lerp(p0, p1, t);
        Vector3 v1 = Vector3.Lerp(p1, p2, t);
        Vector3 v2 = Vector3.Lerp(p2, p3, t);
        Vector3 v3 = Vector3.Lerp(p3, p4, t);

        Vector3 v4 = Vector3.Lerp(v0, v1, t);
        Vector3 v5 = Vector3.Lerp(v1, v2, t);
        Vector3 v6 = Vector3.Lerp(v2, v3, t);

        Vector3 v7 = Vector3.Lerp(v4, v5, t);
        Vector3 v8 = Vector3.Lerp(v5, v6, t);

        return Vector3.Lerp(v7, v8, t);
    }


    // itween 특정 포인트들을 찍으면서 곡선이동
    public float delayTime;
    public float elapsedTime;

    public void onPath()
    {
        float moveTime = 50f;
        this.transform.position = arrPos[0];
        Hashtable hash2 = iTween.Hash("path", arrPos, "time", moveTime, "easeType", iTween.EaseType.easeOutSine, "delay", delayTime );
        iTween.MoveTo(this.gameObject, hash2);
    }
}
