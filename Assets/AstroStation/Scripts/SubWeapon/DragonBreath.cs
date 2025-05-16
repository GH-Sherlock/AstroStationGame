using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreath : Weapon
{
    [SerializeField] private float m_angle;

    private List<Enermy> m_enermyList;
    private List<Enermy> m_hitEnermyList;



    public override void Awake()
    {
        base.Awake();
        m_weaponLevel = 1f;
        m_damage = 7f;
        m_enermyList = GameManager.instance.enermyPool.arrEnermy;

        PolygonCollider2D col = this.gameObject.AddComponent<PolygonCollider2D>();
        col.isTrigger = true;
        col.SetPath(0, CalculateBoundary());
    }

    // Update is called once per frame
    void Update()
    {

        //View();
    }

    public override void Upgrade()
    {
        float maxLevel = GameManager.instance.SubWeaponMaxLevel;
        if (m_weaponLevel >= maxLevel)
            return;

        m_weaponLevel++;


        switch (m_weaponLevel)
        {
            case 2:
                m_damage = 15f;
                break;
            case 3:
                m_damage = 30f;
                break;
        }
    }

    private Vector2 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.z;
        return new Vector2(Mathf.Sin(_angle * Mathf.Deg2Rad), Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private Vector2[] CalculateBoundary()
    {
        Vector2[] result = new Vector2[3];

        Vector2 leftBoundary;
        Vector2 rightBoundary;

        Vector2 leftPoint;
        Vector2 rightPoint;

        for (int i = 0; i <= m_angle; i++)
        {
            leftBoundary = BoundaryAngle(-i * 0.5f);
            rightBoundary = BoundaryAngle(i * 0.5f);
        }


        leftBoundary = BoundaryAngle(-m_angle * 0.5f);
        rightBoundary = BoundaryAngle(m_angle * 0.5f);

        leftPoint = (leftBoundary * m_maximumRange);
        rightPoint = (rightBoundary * m_maximumRange);

        result[0] = Vector2.zero;
        result[1] = leftPoint;
        result[2] = rightPoint;

        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        CharacterStatus stat = collision.GetComponent<CharacterStatus>();

        if (stat)
        {
            collision.GetComponent<CharacterStatus>().Damaged(m_damage);
        }
    }

    private void View()
    {
        Vector2 leftBoundary;
        Vector2 rightBoundary;

        Vector2 drawLeftPosition;
        Vector2 drawRightPosition;

        for (int i = 0; i <= m_angle; i++)
        {
            leftBoundary = BoundaryAngle(-i * 0.5f);
            rightBoundary = BoundaryAngle(i * 0.5f);

            drawLeftPosition = (Vector2)m_firePos.position + (leftBoundary * m_maximumRange);
            drawRightPosition = (Vector2)m_firePos.position + (rightBoundary * m_maximumRange);
            Debug.DrawLine(m_firePos.position, drawLeftPosition, Color.red);
            Debug.DrawLine(m_firePos.position, drawRightPosition, Color.red);
        }


        leftBoundary = BoundaryAngle(-m_angle * 0.5f);
        rightBoundary = BoundaryAngle(m_angle * 0.5f);

        drawLeftPosition = (Vector2)m_firePos.position + (leftBoundary * m_maximumRange);
        drawRightPosition = (Vector2)m_firePos.position + (rightBoundary * m_maximumRange);

        //Debug.DrawLine(drawLeftPosition, drawRightPosition, Color.red);
        Debug.DrawLine(m_firePos.position, drawLeftPosition, Color.red);
        Debug.DrawLine(m_firePos.position, drawRightPosition, Color.red);
        Debug.DrawLine(m_firePos.position, (Vector2)m_firePos.position + (Vector2.up * m_maximumRange), Color.red);

        if(m_enermyList.Count <= 0)
        {
            m_enermyList = GameManager.instance.enermyPool.arrEnermy;
        }

        foreach(Enermy e in m_enermyList)
        {
            if(e.Life >= 0)
            {
                Vector2 dir = (e.transform.position - m_firePos.position).normalized;
                float angle = Vector2.Angle(dir, m_firePos.up);

                if(angle < m_angle * 0.5f)
                {
                    RaycastHit2D hit = Physics2D.Raycast(m_firePos.position, dir, m_maximumRange);

                    if(hit)
                    {
                        //hit.collider.GetComponent<Enermy>().Damaged(m_damage);
                    }
                }
            }
        }
    }

    public override void InitBullet()
    {
        m_isFire = false;
        this.gameObject.SetActive(false);
    }
}
