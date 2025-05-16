using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : Weapon
{
    private Transform m_target;
    private float m_rotSpeed = 1f;
    [SerializeField] private float m_minRotSpeed;
    [SerializeField] private float m_maxRotSpeed;

    public override void Awake()
    {
        base.Awake();
        m_weaponLevel = 1f;
        m_damage = 2f;
        m_rotSpeed = m_minRotSpeed;
    }


    private void Update()
    {
        if (!m_isFire)
            return;

        float move = m_speed * timeManager.GetTimeScale();
        m_moveDistance += move;

        if(m_target == null || m_target.gameObject.activeSelf == false || !m_target.parent.gameObject.activeSelf)
        {
            NonTargetUpdate(move);
            return;
        }

        if(m_target.GetComponent<CharacterStatus>().hp <= 0)
        {
            m_target = null;
            NonTargetUpdate(move);
            return;
        }


        Vector3 toDir = (m_target.transform.position - this.transform.position).normalized;
        float dot = Vector3.Dot(this.transform.up, toDir);

        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Vector3 cross = Vector3.Cross(this.transform.up, toDir);

        if(cross.z < 0)
        {
            angle = this.transform.rotation.eulerAngles.z - m_rotSpeed;
        }
        else
        {
            angle = this.transform.rotation.eulerAngles.z + m_rotSpeed;
        }

        m_rotSpeed += m_rotSpeed * timeManager.GetTimeScale();
        m_rotSpeed = Mathf.Clamp(m_rotSpeed, m_minRotSpeed, m_maxRotSpeed);


        this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        this.transform.Translate(Vector3.up * move);
    }

    public override void Upgrade()
    {
        float maxLevel = GameManager.instance.SubWeaponMaxLevel;
        if (m_weaponLevel >= maxLevel)
            return;

        m_weaponLevel++;
    }

    public Transform Target
    {
        get { return m_target; }
        set { m_target = value; }
    }

    public float RotSpeed
    {
        get { return m_rotSpeed; }
        set { m_rotSpeed = value; }
    }

    public override void Fire(Transform firePos, Quaternion dir)
    {
        base.Fire(firePos, dir);
        m_rotSpeed = m_minRotSpeed;
    }

    private void NonTargetUpdate(float moveValue)
    {
        this.transform.Translate(Vector3.up * moveValue);
        if (m_moveDistance >= m_maximumRange)
        {
            InitBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        CharacterStatus stat = collision.GetComponent<CharacterStatus>();

        if (stat)
        {
            collision.GetComponent<CharacterStatus>().Damaged(m_damage);
            collision.GetComponent<CharacterStatus>().IsLockOn = false;
            InitBullet();
            m_target = null;
        }
    }
}
