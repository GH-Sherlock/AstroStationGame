using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma : Weapon
{
    private float m_fDis = 0f;
    private bool m_isHit = false;

    private int m_lineSize = 100;
    private LineRenderer m_plasmaLine;
    private EdgeCollider2D m_edgeCollider;
    private Vector2[] m_colVec3;
    private EnermyPool m_enermyPool;

    public Vector3 target;
    private float fAngle = 0f;

    public override void Awake()
    {
        base.Awake();
        m_weaponLevel = 1f;
        m_damage = 2f;

        m_enermyPool = GameManager.instance.enermyPool;

        m_plasmaLine = this.GetComponent<LineRenderer>();
        m_edgeCollider = this.GetComponent<EdgeCollider2D>();
        m_plasmaLine.startWidth = 2f;
        m_plasmaLine.endWidth = 2f;

        m_plasmaLine.positionCount = m_lineSize;
        m_colVec3 = new Vector2[m_lineSize];

        for (int i = 0; i < m_lineSize; i++)
            m_plasmaLine.SetPosition(i, Vector3.zero);

    }

    private void Update()
    {
        if (!m_isFire)
        {
            fAngle = 0f;
            return;
        }

        if (m_isHit)
        {
            this.transform.localScale = new Vector3(1f, m_fDis, 1f);
        }
        else
        {
            //homingA();
            homingB();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy"))
        {

            collision.GetComponent<CharacterStatus>().Damaged(m_damage * timeManager.GetDamageOverTime());
        }
    }

    public override void Upgrade()
    {
        
    }

    private void homingA()
    {
        //if (m_moveDistance >= m_maximumRange)
        //    return;

        fAngle = 0f;
        float move = m_speed * timeManager.GetTimeScale();
        m_moveDistance += move;

        float splitDistance = m_moveDistance / m_lineSize;
        target = m_enermyPool.arrEnermy[0].transform.position;

        if (m_moveDistance >= m_maximumRange)
        {
            m_moveDistance = m_maximumRange;
        }

        Vector2 v3Start = m_firePos.position;
        Vector2 v3Mid = new Vector2(v3Start.x, target.y);
        Vector2 v3End = target;

        float t;
        Vector2 p1, p2, p3;

        for (int i = 0; i < m_lineSize; i++)
        {
            t = (float)i / ((float)m_lineSize - 1f);
            p1 = Vector2.Lerp(v3Start, v3Mid, t);
            p2 = Vector2.Lerp(v3Mid, v3End, t);
            p3 = Vector2.Lerp(p1, p2, t);

            p3.x -= this.transform.position.x;
            p3.y -= this.transform.position.y;
            m_plasmaLine.SetPosition(i, p3);
            m_colVec3.SetValue(p3, i);
        }

        m_edgeCollider.points = m_colVec3;
    }

    private void homingB()
    {
        float move = m_speed * timeManager.GetTimeScale();
        m_moveDistance += move;

        target = m_enermyPool.arrEnermy[0].transform.position;

        Vector3 toDir = (target - m_firePos.position).normalized;

        float dot = Vector3.Dot(this.transform.up, toDir);

        Debug.Log(dot);
        if(dot < 1f)
        {
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            Vector3 cross = Vector3.Cross(this.transform.up, toDir);
            //Debug.Log(cross);
            if(cross.z < 0)
            {

            }
            else
            {

            }
        }


        if (m_moveDistance >= m_maximumRange)
        {
            m_moveDistance = m_maximumRange;
        }


        for (int i = 0; i < m_lineSize; i++)
        {
            //m_plasmaLine.SetPosition(i, curPosition);

        }

    }
}
