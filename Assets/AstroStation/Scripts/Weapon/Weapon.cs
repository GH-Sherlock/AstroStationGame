using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected float m_damage;
    [SerializeField] protected float m_speed;
    [SerializeField] protected float m_maximumRange;
    [SerializeField] protected AudioClip m_hitSfxClip;
    protected Sprite m_spriteImage;
    protected TimeManager timeManager;

    protected Transform m_tfBulletPool;
    protected float m_weaponLevel;
    protected bool m_isFire = false;
    protected float m_moveDistance = 0f;
    public Transform m_firePos;

    //protected Vector3 m_fireDir;

    virtual public void Awake()
    {
        this.timeManager = TimeManager.instance;
    }

    public bool IsFire
    {
        get { return m_isFire; }
        set { m_isFire = value; }
    }

    virtual public void Fire(Transform firePos, Quaternion dir)
    {
        m_moveDistance = 0f;
        m_isFire = true;
        this.transform.localRotation = dir;
        this.transform.position = firePos.position;
        this.gameObject.SetActive(true);
    }

    public void BulletDestroy()
    {
        m_isFire = false;
        this.gameObject.SetActive(false);
    }

    public float GetDamage()
    {
        return m_damage;
    }
    public void SetBulletPool(Transform pool)
    {
        m_tfBulletPool = pool;
    }

    virtual public void InitBullet()
    {
        m_moveDistance = 0f;
        m_isFire = false;
        this.transform.SetParent(m_tfBulletPool);
        this.gameObject.SetActive(false);
    }

    virtual public void HitSFXPlay()
    {
        if (!m_hitSfxClip)
            return;

        SoundController.instance.PlaySFX(m_hitSfxClip);
    }

    virtual public void Upgrade() { }
}
